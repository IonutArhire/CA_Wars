import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';

import 'rxjs/add/observable/fromEvent';
import 'rxjs/add/operator/takeUntil';
import 'rxjs/add/operator/switchMap';
import 'rxjs/add/operator/pairwise';

import { IPlayerResource } from '../resources/player-resources';
import { IDimensionsResources } from '../resources/dimensions-resources';
import { Point } from '../entities/point-model';

import { saveAs } from 'file-saver';

@Injectable()
export class MatchService {
  private _canvas: HTMLCanvasElement;
  private _ctx: CanvasRenderingContext2D;
  
  private _cells: number[][];
  private _dimensions: IDimensionsResources;
  private _cellSize: number;

  private _playerResources: Array<IPlayerResource>;

  private _center: [number, number];

  private _marginTop: number;
  private _marginBottom: number;

  private _currGameStateIdx: number;
  private _generations: Array<number[][]>;

  private _simulationInterval: NodeJS.Timer;

  private _map: number[][];

  private _eraserMode: boolean;

  constructor() {
    this._marginBottom = 216;
    this._cellSize = 30;
    this._center = [0, 0];
    this._currGameStateIdx = 0;
    this._eraserMode = false;
  }



  /*
  * Initialization and Rendering Section.
  */

  public init(canvas: HTMLCanvasElement, 
              toolbar: HTMLDivElement, 
              dimensions: IDimensionsResources, 
              playerResources: Array<IPlayerResource>,
              map: number[][]): void {

    this._canvas = canvas;
    this._dimensions = dimensions;
    this._marginTop = toolbar.offsetHeight;
    this._playerResources = playerResources;
    this._map = map;
    this._cells = map;

    let width = this._canvas.width = window.innerWidth;
    let height = this._canvas.height = window.innerHeight - this._marginTop - this._marginBottom;

    this._ctx = this._canvas.getContext('2d');

    this.updateCenter(width, height);

    this._ctx.strokeStyle = "black";
    this._ctx.lineWidth = 1.0;

    this.disableContextMenu();
  }

  public getCells(): number[][] {
    return this._cells;
  }
  
  public disableContextMenu(): void {
    this._canvas.oncontextmenu = function (e) {
      e.preventDefault();
    };
  }

  public drawCell(base: Point, i: number, j: number): void {
    let x = base.x + j * this._cellSize;
    let y = base.y + i * this._cellSize;
    let cellValue = this._cells[i][j]

    if (cellValue == -1) {
      this._ctx.fillStyle = "white";
    }
    else if (cellValue == -2) {
      this._ctx.fillStyle = "#bcbcbc"; //grey
    }
    else {
      this._ctx.fillStyle = this._playerResources[cellValue].color;
    }

    this._ctx.fillRect(x, y, this._cellSize, this._cellSize);
    this._ctx.strokeRect(x, y, this._cellSize, this._cellSize);
  }

  public updateCell(i: number, j: number, assignedNum: number): void {
    if (this._cells[i][j] != -2) {
      if (this._eraserMode) {
        this._cells[i][j] = -1;
      }
      else {
        this._cells[i][j] = assignedNum;
      }
    }
    let base = this.getGridBasePoint();
    this.drawCell(base, i, j);
  }

  public drawGrid(): void {
    let base = this.getGridBasePoint();

    for (var i: number = 0; i < this._dimensions.height; i++) {
      for (var j: number = 0; j < this._dimensions.width; j++) {
        this.drawCell(base, i, j);
      }
    }
  }

  public getGridBasePoint(): Point {
    return new Point(0 - this._dimensions.width / 2 * this._cellSize,
                     0 - this._dimensions.height / 2 * this._cellSize);
  }

  public getGridTopLeft(): Point {
    return new Point(this._center["0"] - this._dimensions.width / 2 * this._cellSize,
                     this._center["1"] - this._dimensions.height / 2 * this._cellSize);
  }

  public clearCanvas(): void {
    this._ctx.save();

    this._ctx.setTransform(1, 0, 0, 1, 0, 0);
    this._ctx.clearRect(0, 0, this._canvas.width, this._canvas.height);

    this._ctx.restore();
  }

  public translateAndUpdate(x: number, y: number): void {
    this._ctx.translate(x, y);
    this._center["0"] += x;
    this._center["1"] += y;
  }

  public updateCenter(width: number, height: number): void {
    this._center["0"] = width / 2;
    this._center["1"] = height / 2;
    
    this._ctx.translate(this._center["0"], this._center["1"]);
  }

  public resizeCanvas(windowWidth: number, windowHeight: number): void {
    this.clearCanvas();

    let width = this._canvas.width = windowWidth;
    let height = this._canvas.height = windowHeight - this._marginTop - this._marginBottom ;
    this.updateCenter(width, height);

    this.drawGrid();
  }

  public updatePlayerResources(playerResources: Array<IPlayerResource>): void {
    this._playerResources = playerResources;
  }



  /*
  * Game Simulation Section.
  */

  public renderSimulationStep(): void {
    this._cells = this._generations[this._currGameStateIdx];
    this.drawGrid();
  }

  public initSimulation(generations: Array<number[][]>): void {
    this._generations = generations;
    this._cells = this._generations[this._currGameStateIdx];
    this.drawGrid();
  }

  public saveGenerationsAsImgs() {
    for (let idx = 0; idx < this._generations.length; idx++) {
      this._cells = this._generations[idx];
      this.drawGrid();

      this._canvas.toBlob(function(blob) {
        saveAs(blob, idx + ".png");
      });
    }
  }

  public startSimulation(): void {
    this._simulationInterval = setInterval(() => {
    if (this._currGameStateIdx < this._generations.length - 1) {
      this._currGameStateIdx += 1;
        this.renderSimulationStep();
      }
    }, 60);
  }

  public stopSimulation(): void {
    clearInterval(this._simulationInterval);
  }

  public skipSimulationBack(): void {
    this.stopSimulation();
    this._currGameStateIdx = 0;
    this.renderSimulationStep();
  }

  public skipSimulationForward(): void {
    this.stopSimulation();
    this._currGameStateIdx = this._generations.length - 1;
    this.renderSimulationStep();
  }

  public stepSimulationBack(): void {
    this.stopSimulation();
    if(this._currGameStateIdx > 0) {
      this._currGameStateIdx -= 1;
      this.renderSimulationStep();
    }
  }

  public stepSimulationForward(): void {
    this.stopSimulation();
    if (this._currGameStateIdx < this._generations.length - 1) {
      this._currGameStateIdx += 1;
      this.renderSimulationStep();
    }
  }

  public resetMatch(): void {
    this.stopSimulation();
    this._currGameStateIdx = 0;
    this._cells = this._map;
    this.clearCanvas();
    this.drawGrid();
  }



  /*
  * Events Handling Section.
  */

  public captureEvents(assignedNum: number): void {
    this.captureLeftClick(assignedNum);
    this.captureLeftDrag(assignedNum);
    this.captureRightDrag();
    this.captureMouseWheel();
  }

  public captureLeftClick(assignedNum: number): void {
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

          let x = pos.x - gridTopLeft.x;
          let y = pos.y - gridTopLeft.y;

          let i = Math.floor(y / this._cellSize);
          let j = Math.floor(x / this._cellSize);

          if (i >= 0 && i < this._dimensions.height && j >= 0 && j < this._dimensions.width) {
            this.updateCell(i, j, assignedNum);
          }
        }
      });
  }

  public captureLeftDrag(assignedNum: number): void {
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

          let x = pos.x - gridTopLeft.x;
          let y = pos.y - gridTopLeft.y;

          let i = Math.floor(y / this._cellSize);
          let j = Math.floor(x / this._cellSize);

          if (i >= 0 && i < this._dimensions.height && j >= 0 && j < this._dimensions.width) {
            this.updateCell(i, j, assignedNum);
          }
        }
      });
  }

  public captureRightDrag(): void {
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
          this.drawGrid();
        }
        
      });
  }

  public captureMouseWheel(): void {
    Observable
      .fromEvent(this._canvas, 'mousewheel')
      .subscribe((res: MouseWheelEvent) => {
        if(this._cellSize >= 10 || res.deltaY < 0) {
          this._cellSize += -res.deltaY / 50;
          this.clearCanvas();
          this.drawGrid();
        }
      })
  }

  /*
  * Modes
  */ 

  public activateEraserMode(): void {
    this._eraserMode = true;
  }

  public deactivateEraserMode(): void {
    this._eraserMode = false;
  }

  public activateGridMode(): void {
    this._ctx.strokeStyle = "black";
    this.clearCanvas();
    this.drawGrid();
  }

  public deactivateGridMode(): void {
    this._ctx.strokeStyle = "white";
    this.clearCanvas();
    this.drawGrid();
  }
}
