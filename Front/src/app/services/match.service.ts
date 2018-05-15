import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';

import 'rxjs/add/observable/fromEvent';
import 'rxjs/add/operator/takeUntil';
import 'rxjs/add/operator/switchMap';
import 'rxjs/add/operator/pairwise';

import { IPlayerResource } from '../resources/player-resources';
import { IDimensionsResources } from '../resources/dimensions-resources';
import { Point } from '../entities/point-entity';

@Injectable()
export class MatchService {

  private _canvas: HTMLCanvasElement;
  private _ctx: CanvasRenderingContext2D;
  
  private _cells: number[][];
  private _dimensions: IDimensionsResources;
  private _cellSize: number;

  private _center: [number, number];

  private _marginTop: number;
  private _marginBottom: number;

  private _currGameStateIdx: number;
  private _generations: Array<number[][]>;

  private _interval: any;

  constructor() {
    this._marginBottom = 216;
    this._cellSize = 30;
    this._center = [0, 0];
    this._currGameStateIdx = 0;
  }



  /*
  * Initialization and Rendering Section.
  */

  init(canvas: HTMLCanvasElement, toolbar: HTMLDivElement, dimensions: IDimensionsResources) {
    this._canvas = canvas;
    this._dimensions = dimensions;
    this._cells = this.initializeCells();
    this._marginTop = toolbar.offsetHeight;

    let width = this._canvas.width = window.innerWidth;
    let height = this._canvas.height = window.innerHeight - this._marginTop - this._marginBottom;

    this._ctx = this._canvas.getContext('2d');

    this.updateCenter(width, height);

    this._ctx.strokeStyle = "black";
    this._ctx.lineWidth = 1.0;

    this.disableContextMenu();
  }

  initializeCells(): number[][] {
    let cells = []
    for (var i: number = 0; i < this._dimensions.height; i++) {
      cells[i] = [];
      for (var j: number = 0; j < this._dimensions.width; j++) {
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

  drawCell(base: Point, i: number, j: number, playerResources: Array<IPlayerResource>) {
    let x = base.x + j * this._cellSize;
    let y = base.y + i * this._cellSize;
    let cellValue = this._cells[i][j]
    if (cellValue !== -1) {
      this._ctx.fillStyle = playerResources[cellValue].color;
    }
    else {
      this._ctx.fillStyle = "white";
    }
    this._ctx.fillRect(x, y, this._cellSize, this._cellSize);
    this._ctx.strokeRect(x, y, this._cellSize, this._cellSize);
  }

  updateCell(i: number, j: number, playerNum: number, playerResources: Array<IPlayerResource>) {
      this._cells[i][j] = playerNum;
      let base = this.getGridBasePoint();
      this.drawCell(base, i, j, playerResources);
  }

  drawGrid(playerResources: Array<IPlayerResource>) {
    let base = this.getGridBasePoint();

    for (var i: number = 0; i < this._dimensions.height; i++) {
      for (var j: number = 0; j < this._dimensions.width; j++) {
        this.drawCell(base, i, j, playerResources);
      }
    }
  }

  getGridBasePoint(): Point {
    return new Point(0 - this._dimensions.width / 2 * this._cellSize,
                     0 - this._dimensions.height / 2 * this._cellSize);
  }

  getGridTopLeft(): [number, number] {
    return [this._center["0"] - this._dimensions.width / 2 * this._cellSize,
            this._center["1"] - this._dimensions.height / 2 * this._cellSize];
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

  updateCenter(width: number, height: number) {
    this._center["0"] = width / 2;
    this._center["1"] = height / 2;
    
    this._ctx.translate(this._center["0"], this._center["1"]);
  }

  resizeCanvas(event, playerResources) {
    this.clearCanvas();

    console.log(event.target.innerWidth, event.target.innerHeight);

    let width = this._canvas.width = event.target.innerWidth;
    let height = this._canvas.height = event.target.innerHeight - this._marginTop - this._marginBottom ;
    this.updateCenter(width, height);

    this.drawGrid(playerResources);
  }




  /*
  * Game Simulation Section.
  */

  runGame(generations: Array<number[][]>, playerResources: Array<IPlayerResource>) {
    this._generations = generations;
    this._cells = generations[this._currGameStateIdx];
    this.drawGrid(playerResources);
  }
  
  resetMatch(playerResources: Array<IPlayerResource>) {
    clearInterval(this._interval);
    this._currGameStateIdx = 0;
    this._cells = this.initializeCells();
    this.clearCanvas();
    this.drawGrid(playerResources);
  }

  startSimulation(playerResources: Array<IPlayerResource>) {
    this._interval = setInterval(() => {
      if (this._currGameStateIdx < this._generations.length - 1) {
        this._currGameStateIdx += 1;
        this._cells = this._generations[this._currGameStateIdx];
        this.drawGrid(playerResources);
      }
    }, 60);
  }

  stopSimulation() {
    clearInterval(this._interval);
  }

  skipSimulationBack(playerResources: Array<IPlayerResource>) {
    clearInterval(this._interval);
    this._currGameStateIdx = 0;
    this._cells = this._generations[this._currGameStateIdx];
    this.drawGrid(playerResources);
  }

  skipSimulationForward(playerResources: Array<IPlayerResource>) {
    clearInterval(this._interval);
    this._currGameStateIdx = this._generations.length - 1;
    this._cells = this._generations[this._currGameStateIdx];
    this.drawGrid(playerResources);
  }

  stepSimulationBack(playerResources: Array<IPlayerResource>) {
    clearInterval(this._interval);
    if(this._currGameStateIdx > 0) {
      this._currGameStateIdx -= 1;
      this._cells = this._generations[this._currGameStateIdx];
      this.drawGrid(playerResources);
    }
  }

  stepSimulationForward(playerResources: Array<IPlayerResource>) {
    clearInterval(this._interval);
    if (this._currGameStateIdx < this._generations.length - 1) {
      this._currGameStateIdx += 1;
      this._cells = this._generations[this._currGameStateIdx];
      this.drawGrid(playerResources);
    }
  }





  /*
  * Events Handling Section.
  */

  captureEvents(playerResources: Array<IPlayerResource>, playerNum: number) {
    this.captureLeftClick(playerResources, playerNum);
    this.captureLeftDrag(playerResources, playerNum);
    this.captureRightDrag(playerResources);
    this.captureMouseWheel(playerResources);
  }

  captureLeftClick(playerResources: Array<IPlayerResource>, playerNum: number) {
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

          if (i >= 0 && i < this._dimensions.height && j >= 0 && j < this._dimensions.width) {
            this.updateCell(i, j, playerNum, playerResources);
          }
        }
      });
  }

  captureLeftDrag(playerResources: Array<IPlayerResource>, playerNum: number) {
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

          if (i >= 0 && i < this._dimensions.height && j >= 0 && j < this._dimensions.width) {
            this.updateCell(i, j, playerNum, playerResources);
          }
        }
      });
  }

  captureRightDrag(playerResources: Array<IPlayerResource>) {
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

  captureMouseWheel(playerResources: Array<IPlayerResource>) {
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
