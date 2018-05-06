import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';

import 'rxjs/add/observable/fromEvent';
import 'rxjs/add/operator/takeUntil';
import 'rxjs/add/operator/switchMap';
import { IPlayerResources } from '../PlayerResources';

@Injectable()
export class MatchService {

  private _canvas: HTMLCanvasElement;
  private _ctx: CanvasRenderingContext2D;
  
  private _cellSize: number;

  private _center: [number, number];
  private _gridTopLeft: [number, number];

  private _marginBottom: number;

  constructor() {
    this._marginBottom = 50;
    this._cellSize = 30;

    this._center = [0, 0];
    this._gridTopLeft = [0, 0];
  }

  init(canvas: HTMLCanvasElement, nrCells: number) {
    this._canvas = canvas;

    let width = this._canvas.width = window.innerWidth;
    let height = this._canvas.height = window.innerHeight - this._marginBottom;

    this._ctx = this._canvas.getContext('2d');

    this._center["0"] = width / 2;
    this._center["1"] = height / 2;

    this._gridTopLeft["0"] = this._center["0"] - nrCells / 2 * this._cellSize;
    this._gridTopLeft["1"] = this._center["1"] - nrCells / 2 * this._cellSize;

    this._ctx.translate(this._center["0"], this._center["1"]);

    this._ctx.strokeStyle = "black";
    this._ctx.lineWidth = 1.0;
  }

  drawCell(base, cells: number[][], i: number, j: number, playerResources: IPlayerResources) {
    let x = base + j * this._cellSize;
    let y = base + i * this._cellSize;
    let cellValue = cells[i][j]
    if (cellValue !== -1) {
      this._ctx.fillStyle = playerResources.allPlayersRes[cellValue].color;
    }
    else {
      this._ctx.fillStyle = "white";
    }
    this._ctx.fillRect(x, y, this._cellSize, this._cellSize);
    this._ctx.strokeRect(x, y, this._cellSize, this._cellSize);
  }

  updateCell(cells: number[][], nrCells: number, i: number, j: number, playerNum: number, playerResources: IPlayerResources) {
      cells[i][j] = playerNum;
      let base = 0 - nrCells / 2 * this._cellSize;
      this.drawCell(base, cells, i, j, playerResources);
  }

  drawGrid(cells: number[][], nrCells: number, playerResources: IPlayerResources) {
    let base = 0 - nrCells / 2 * this._cellSize;

    for (var i: number = 0; i < nrCells; i++) {
      for (var j: number = 0; j < nrCells; j++) {
        this.drawCell(base, cells, i, j, playerResources);
      }
    }
  }

  captureEvents(cells: number[][], nrCells: number, playerResources: IPlayerResources, playerNum: number) {
    Observable
      .fromEvent(this._canvas, 'mousedown')
      .subscribe((res: MouseEvent) => {
        const rect = this._canvas.getBoundingClientRect();

        const pos = {
          x: res.clientX - rect.left,
          y: res.clientY - rect.top
        };

        let x = pos.x - this._gridTopLeft["0"];
        let y = pos.y - this._gridTopLeft["1"];

        let i = Math.floor(y / this._cellSize);
        let j = Math.floor(x / this._cellSize);

        if (i >= 0 && i < nrCells && j >= 0 && j < nrCells) {
          console.log(playerResources);
          console.log(playerResources.allPlayersRes[playerNum].color);
          this.updateCell(cells, nrCells, i, j, playerNum, playerResources);
        }
      });

    Observable
      .fromEvent(this._canvas, 'mousedown')
      .switchMap((e) => {
        return Observable
          .fromEvent(this._canvas, 'mousemove')
          .takeUntil(Observable.fromEvent(this._canvas, 'mouseup'))
          .takeUntil(Observable.fromEvent(this._canvas, 'mouseleave'))
      })
      .subscribe((res: MouseEvent) => {
        const rect = this._canvas.getBoundingClientRect();

        const pos = {
          x: res.clientX - rect.left,
          y: res.clientY - rect.top
        };

        let x = pos.x - this._gridTopLeft["0"];

        let y = pos.y - this._gridTopLeft["1"];

        let i = Math.floor(y / this._cellSize);
        let j = Math.floor(x / this._cellSize);

        if (i > 0 && i < nrCells && j > 0 && j < nrCells) {
          this.updateCell(cells, nrCells, i, j, playerNum, playerResources);
        }
      });
  }

}
