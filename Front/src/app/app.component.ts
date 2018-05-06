import { Component, ViewChild, ElementRef, HostListener } from '@angular/core';
import { HubConnection } from '@aspnet/signalr';

import { Observable } from 'rxjs/Observable';
import {BehaviorSubject} from 'rxjs/BehaviorSubject';
import 'rxjs/add/observable/of'

import { MatchService } from './services/match.service';

import { IPlayerResources } from './PlayerResources';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {

  private _hubConnection: HubConnection

  @ViewChild('playGrid') playGrid: ElementRef;

  private _connected: boolean = false;
  private _connectedStatus: BehaviorSubject<boolean>;

  private _nrCells: number;

  private _playerResources: IPlayerResources;
  private _playerNum: number = -1;

  private _cells: number[][];

  private _gameStates: Array<number[][]>;
  private _currGameStateIdx: number = 0;

  constructor(private _matchService: MatchService) {
    this._nrCells = 20;
    this.initializeCells();
    this._connectedStatus = new BehaviorSubject<boolean>(false);
  }

  getConnectedStatus(): Observable<boolean> {
    return this._connectedStatus.asObservable();
  }

  setConnectedStatus(newValue: boolean) {
    this._connected = newValue;
    this._connectedStatus.next(newValue);
  }

  initializeCells() {
    this._cells = []
    for (var i: number = 0; i < this._nrCells; i++) {
      this._cells[i] = [];
      for (var j: number = 0; j < this._nrCells; j++) {
        this._cells[i][j] = -1;
      }
    }
  }

  ngOnInit() {
    this._hubConnection = new HubConnection('http://localhost:5000/match');

    this._hubConnection.on('SendConnected', (data) => {this.SendConnected(data)});

    this._hubConnection.on('SendDisconnected', (data) => {this.SendDisconnected(data)});

    this._hubConnection.on('SendMessage', (data) => {this.SendMessage(data)});

    this._hubConnection.on('SendGame', (data) => {this.SendGame(data)});

    this._hubConnection
      .start()
      .then(() => this.onHubConnected())
      .catch(err => console.log(err));
  }

  onHubConnected() {
    console.log("connected!");
  }

  SendConnected(playerResources: IPlayerResources) {
    this._playerResources = playerResources;
    this._playerNum = playerResources.number;
    this.setConnectedStatus(true);
  }

  SendDisconnected(data) {
    console.log(data);
    console.log('disconnected');
  }

  SendMessage(data) {
    console.log(data);
  }

  SendGame(data) {
    console.log(data);
    this._gameStates = data;

    setInterval(() => {
      if (this._currGameStateIdx != this._gameStates.length) {
        this._cells = this._gameStates[this._currGameStateIdx];
        this._matchService.drawGrid(this._cells, this._nrCells, this._playerResources);
        this._currGameStateIdx += 1;
      }
    }, 60);
  }

  sendConfig() {
    this._hubConnection.invoke('send', this._cells);
  }

  onResize(event) {
    //console.log(event.target.innerWidth); 
    //console.log(event.target.innerHeight); 
    //this.canvas.width = event.target.innerWidth;
    //this.canvas.height = event.target.innerHeight;
  }

  ngAfterViewInit() {
    this.getConnectedStatus().subscribe((_connected) => {
      if (_connected) {
        this._matchService.init(this.playGrid.nativeElement, this._nrCells);
        this._matchService.drawGrid(this._cells, this._nrCells, this._playerResources);
        this._matchService.captureEvents(this._cells, this._nrCells, this._playerResources, this._playerNum);
      }
    });
  }

}
