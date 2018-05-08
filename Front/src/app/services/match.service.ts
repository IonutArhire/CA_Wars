import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';

import 'rxjs/add/observable/fromEvent';
import 'rxjs/add/operator/takeUntil';
import 'rxjs/add/operator/switchMap';
import 'rxjs/add/operator/pairwise';

import { IPlayerResources } from '../models/player-resources';

@Injectable()
export class MatchService {

  private _canvas: HTMLCanvasElement;
  private _ctx: CanvasRenderingContext2D;
  
  private _cells: number[][];
  private _nrCells: number;
  private _cellSize: number;

  private _center: [number, number];

  private _marginBottom: number;

  constructor() {
    this._marginBottom = 75;
    this._cellSize = 30;
    this._center = [0, 0];
  }

  init(canvas: HTMLCanvasElement, nrCells: number) {
    this._canvas = canvas;
    this._nrCells = nrCells;
    this._cells = this.initializeCells();

    let width = this._canvas.width = window.innerWidth;
    let height = this._canvas.height = window.innerHeight - this._marginBottom;

    this._ctx = this._canvas.getContext('2d');

    this._center["0"] = width / 2;
    this._center["1"] = height / 2;

    this._ctx.translate(this._center["0"], this._center["1"]);

    this._ctx.strokeStyle = "black";
    this._ctx.lineWidth = 1.0;

    this.disableContextMenu();
  }

  initializeCells(): number[][] {
    let cells = []
    for (var i: number = 0; i < this._nrCells; i++) {
      cells[i] = [];
      for (var j: number = 0; j < this._nrCells; j++) {
        cells[i][j] = -1;
      }
    }
    return cells;
  }

  getCells(): number[][] {
    return this._cells;
  }
  
  disableContextMenu() {
    this._canvas.oncontextmenu = function (e) {
      e.preventDefault();
    };
  }

  drawCell(base, i: number, j: number, playerResources: IPlayerResources) {
    let x = base + j * this._cellSize;
    let y = base + i * this._cellSize;
    let cellValue = this._cells[i][j]
    if (cellValue !== -1) {
      this._ctx.fillStyle = playerResources.allPlayersRes[cellValue].color;
    }
    else {
      this._ctx.fillStyle = "white";
    }
    this._ctx.fillRect(x, y, this._cellSize, this._cellSize);
    this._ctx.strokeRect(x, y, this._cellSize, this._cellSize);
  }

  updateCell(i: number, j: number, playerNum: number, playerResources: IPlayerResources) {
      this._cells[i][j] = playerNum;
      let base = 0 - this._nrCells / 2 * this._cellSize;
      this.drawCell(base, i, j, playerResources);
  }

  drawGrid(playerResources: IPlayerResources) {
    let base = 0 - this._nrCells / 2 * this._cellSize;

    for (var i: number = 0; i < this._nrCells; i++) {
      for (var j: number = 0; j < this._nrCells; j++) {
        this.drawCell(base, i, j, playerResources);
      }
    }
  }

  getGridTopLeft(): [number, number] {
    return [this._center["0"] - this._nrCells / 2 * this._cellSize,
            this._center["1"] - this._nrCells / 2 * this._cellSize];
  }

  clearCanvas() {
    this._ctx.save();

    // Use the identity matrix while clearing the canvas
    this._ctx.setTransform(1, 0, 0, 1, 0, 0);
    this._ctx.clearRect(0, 0, this._canvas.width, this._canvas.height);

    this._ctx.restore();
  }

  translateAndUpdate(x, y) {
    this._ctx.translate(x, y);
    this._center["0"] += x;
    this._center["1"] += y;
  }

  
  private _currGameStateIdx: number = 0;

  runGame(generations, winner, playerResources) {
    setInterval(() => {
      if (this._currGameStateIdx < generations.length) {
        this._cells = generations[this._currGameStateIdx];
        this.drawGrid(playerResources);
        this._currGameStateIdx += 1;
      }
    }, 60);
  }

  captureEvents(playerResources: IPlayerResources, playerNum: number) {
    this.captureLeftClick(playerResources, playerNum);
    this.captureLeftDrag(playerResources, playerNum);
    this.captureRightDrag(playerResources);
    this.captureMouseWheel(playerResources);
  }

  captureLeftClick(playerResources: IPlayerResources, playerNum: number) {
    Observable
      .fromEvent(this._canvas, 'mousedown')
      .subscribe((res: MouseEvent) => {
        if(res.which == 1) {
          const rect = this._canvas.getBoundingClientRect();

          const pos = {
            x: res.clientX - rect.left,
            y: res.clientY - rect.top
          };

          let gridTopLeft = this.getGridTopLeft();

          let x = pos.x - gridTopLeft["0"];
          let y = pos.y - gridTopLeft["1"];

          let i = Math.floor(y / this._cellSize);
          let j = Math.floor(x / this._cellSize);

          if (i >= 0 && i < this._nrCells && j >= 0 && j < this._nrCells) {
            this.updateCell(i, j, playerNum, playerResources);
          }
        }
      });
  }

  captureLeftDrag(playerResources: IPlayerResources, playerNum: number) {
    Observable
      .fromEvent(this._canvas, 'mousedown')
      .switchMap((e) => {
        return Observable
          .fromEvent(this._canvas, 'mousemove')
          .takeUntil(Observable.fromEvent(this._canvas, 'mouseup'))
          .takeUntil(Observable.fromEvent(this._canvas, 'mouseleave'))
      })
      .subscribe((res: MouseEvent) => {
        if(res.which == 1) {
          const rect = this._canvas.getBoundingClientRect();

          const pos = {
            x: res.clientX - rect.left,
            y: res.clientY - rect.top
          };

          let gridTopLeft = this.getGridTopLeft();

          let x = pos.x - gridTopLeft["0"];
          let y = pos.y - gridTopLeft["1"];

          let i = Math.floor(y / this._cellSize);
          let j = Math.floor(x / this._cellSize);

          if (i >= 0 && i < this._nrCells && j >= 0 && j < this._nrCells) {
            this.updateCell(i, j, playerNum, playerResources);
          }
        }
      });
  }

  captureRightDrag(playerResources: IPlayerResources) {
    Observable
      .fromEvent(this._canvas, 'mousedown')
      .switchMap((e) => {
        return Observable
          .fromEvent(this._canvas, 'mousemove')
          .takeUntil(Observable.fromEvent(this._canvas, 'mouseup'))
          .takeUntil(Observable.fromEvent(this._canvas, 'mouseleave'))
          .pairwise()
      })
      .subscribe((res: [MouseEvent, MouseEvent]) => {
        if(res[0].which == 3 && res[1].which == 3) {
          const rect = this._canvas.getBoundingClientRect();

          const prevPos = {
            x: res[0].clientX - rect.left,
            y: res[0].clientY - rect.top
          };

          const currentPos = {
            x: res[1].clientX - rect.left,
            y: res[1].clientY - rect.top
          };

          this.clearCanvas();
          this.translateAndUpdate(currentPos.x - prevPos.x, currentPos.y - prevPos.y);
          this.drawGrid(playerResources);
        }
        
      });
  }

  captureMouseWheel(playerResources: IPlayerResources) {
    Observable
      .fromEvent(this._canvas, 'mousewheel')
      .subscribe((res: MouseWheelEvent) => {
        if(this._cellSize >= 10 || res.deltaY < 0) {
          this._cellSize += -res.deltaY / 50;
          this.clearCanvas();
          this.drawGrid(playerResources);
        }
      })
  }

}
