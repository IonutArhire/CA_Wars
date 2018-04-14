import { Component, ViewChild, ElementRef, HostListener } from '@angular/core';
import { HubConnection } from '@aspnet/signalr';
import { Observable } from 'rxjs/Observable';

import 'rxjs/add/observable/fromEvent';
import 'rxjs/add/operator/takeUntil';
import 'rxjs/add/operator/switchMap';
import { PlayerResource } from './PlayerResource';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {

  private hubConnection: HubConnection

  @ViewChild('playGrid') playGrid: ElementRef;
  private canvas: HTMLCanvasElement;
  private ctx: CanvasRenderingContext2D;

  private connected: boolean = false;

  private center: [number, number];
  private gridTopLeft: [number, number];

  private marginBottom: number;
  private nrCells: number;
  private cellSize: number;

  private playerResources: Array<object>;
  private playerNum: number = -1;

  private cells: number[][];

  private mouseDown: boolean = false;

  private game: Array<number[][]>;
  private counter: number = 0;

  constructor() {
    this.marginBottom = 50;
    this.nrCells = 20;
    this.cellSize = 30;

    this.center = [0, 0];
    this.gridTopLeft = [0, 0];

    this.initializeCells();
  }

  initializeCells() {
    this.cells = []
    for (var i: number = 0; i < this.nrCells; i++) {
      this.cells[i] = [];
      for (var j: number = 0; j < this.nrCells; j++) {
        this.cells[i][j] = -1;
      }
    }
  }

  ngOnInit() {
    this.hubConnection = new HubConnection('http://localhost:5000/match');

    this.hubConnection.on('SendConnected', playerResources => {
      this.playerResources = playerResources;
      this.playerNum = playerResources.number;
    });

    this.hubConnection.on('SendDisconnected', data => {
      console.log(data);
      console.log('disconnected');
    });

    this.hubConnection.on('SendMessage', data => {
      console.log(data);
    });

    this.hubConnection.on('SendGame', data => {
      console.log(data);
      this.game = data;

      setInterval(() => {
        if (this.counter != this.game.length) {
          this.cells = this.game[this.counter];
          this.drawGrid();
          this.counter += 1;
        }
      }, 60);
    });

    this.hubConnection
      .start()
      .then(() => this.onHubConnected())
      .catch(err => console.log(err));
  }

  onHubConnected() {
    this.connected = true;
  }

  sendConfig() {
    this.hubConnection.invoke('send', this.cells);
  }

  onResize(event) {
    //console.log(event.target.innerWidth); 
    //console.log(event.target.innerHeight); 
    //this.canvas.width = event.target.innerWidth;
    //this.canvas.height = event.target.innerHeight;
  }

  drawCell(base, i, j) {
    let x = base + j * this.cellSize;
    let y = base + i * this.cellSize;
    if (this.cells[i][j] !== -1) {
      this.ctx.fillStyle = this.playerResources["allPlayersRes"][this.cells[i][j]]["color"];
    }
    else {
      this.ctx.fillStyle = "white";
    }
    this.ctx.fillRect(x, y, this.cellSize, this.cellSize);
    this.ctx.strokeRect(x, y, this.cellSize, this.cellSize);
  }

  drawGrid() {
    let base = 0 - this.nrCells / 2 * this.cellSize;

    for (var i: number = 0; i < this.nrCells; i++) {
      for (var j: number = 0; j < this.nrCells; j++) {
        this.drawCell(base, i, j);
      }
    }

  }

  ngAfterViewInit(): void {
    this.canvas = <HTMLCanvasElement>this.playGrid.nativeElement;
    let width = this.canvas.width = window.innerWidth;
    let height = this.canvas.height = window.innerHeight - this.marginBottom;

    this.ctx = this.canvas.getContext('2d');

    this.center["0"] = width / 2;
    this.center["1"] = height / 2;

    this.gridTopLeft["0"] = this.center["0"] - this.nrCells / 2 * this.cellSize;
    this.gridTopLeft["1"] = this.center["1"] - this.nrCells / 2 * this.cellSize;

    this.ctx.translate(this.center["0"], this.center["1"]);

    this.ctx.strokeStyle = "black";
    this.ctx.lineWidth = 1.0;

    this.drawGrid();
    this.captureEvents();
  }

  captureEvents() {
    Observable
      .fromEvent(this.canvas, 'mousedown')
      .subscribe((res: MouseEvent) => {
        const rect = this.canvas.getBoundingClientRect();

        const pos = {
          x: res.clientX - rect.left,
          y: res.clientY - rect.top
        };

        let x = pos.x - this.gridTopLeft["0"];
        let y = pos.y - this.gridTopLeft["1"];

        let i = Math.floor(y / this.cellSize);
        let j = Math.floor(x / this.cellSize);

        if (i >= 0 && i < this.nrCells && j >= 0 && j < this.nrCells) {
          this.cells[i][j] = this.playerNum;
          console.log(this.playerResources);
          console.log(this.playerResources["allPlayersRes"][this.playerNum]["color"]);
          this.drawGrid();
        }

      });

    Observable
      .fromEvent(this.canvas, 'mousedown')
      .switchMap((e) => {
        return Observable
          .fromEvent(this.canvas, 'mousemove')
          .takeUntil(Observable.fromEvent(this.canvas, 'mouseup'))
          .takeUntil(Observable.fromEvent(this.canvas, 'mouseleave'))
      })
      .subscribe((res: MouseEvent) => {
        const rect = this.canvas.getBoundingClientRect();

        const pos = {
          x: res.clientX - rect.left,
          y: res.clientY - rect.top
        };

        let x = pos.x - this.gridTopLeft["0"];
        let y = pos.y - this.gridTopLeft["1"];

        let i = Math.floor(y / this.cellSize);
        let j = Math.floor(x / this.cellSize);

        if (i > 0 && i < this.nrCells && j > 0 && j < this.nrCells) {
          this.cells[i][j] = this.playerNum;
          this.drawGrid();
        }

      });
  }

}
