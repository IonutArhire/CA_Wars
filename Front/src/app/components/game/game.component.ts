import { Component, ViewChild, ElementRef, HostListener } from '@angular/core';
import { HubConnection } from '@aspnet/signalr';
import { ActivatedRoute } from '@angular/router';

import { Observable } from 'rxjs/Observable';
import {BehaviorSubject} from 'rxjs/BehaviorSubject';
import 'rxjs/add/observable/of'

import { MatchService } from '../../services/match.service';

import { IPlayerResource } from '../../resources/player-resources';
import { IResources } from '../../resources/resources';
import { IDimensionsResources } from '../../resources/dimensions-resources';

@Component({
  selector: 'app-game',
  templateUrl: './game.component.html',
  styleUrls: ['./game.component.css']
})
export class GameComponent {

  private _hubConnection: HubConnection

  @ViewChild('playGrid') playGrid: ElementRef;
  @ViewChild('toolbar') toolbar: ElementRef;

  private _connected: boolean;
  private _connectedStatus: BehaviorSubject<boolean>;

  private _dimensions: IDimensionsResources;

  private _playerResources: Array<IPlayerResource>;
  private _playerNum: number;

  private _gameKey: number;

  private _isPlaying: boolean;
  private _hasGameArrived: boolean;
  private _hasSent: boolean;


  constructor(private _matchService: MatchService, private _route: ActivatedRoute) {
    this._connectedStatus = new BehaviorSubject<boolean>(false);

    this._connected = false;
    this._playerNum = -1;
    this._isPlaying = false;
    this._hasSent = false;
  }

  getConnectedStatus(): Observable<boolean> {
    return this._connectedStatus.asObservable();
  }

  setConnectedStatus(newValue: boolean) {
    this._connected = newValue;
    this._connectedStatus.next(newValue);
  }

  ngOnInit() {
    this._gameKey = parseInt(this._route.snapshot.paramMap.get('game-key'));

    this._hubConnection = new HubConnection('http://localhost:5000/match');

    this._hubConnection.on('Connected', (data) => {this.SendConnected(data)});

    this._hubConnection.on('Disconnected', (data) => {this.SendDisconnected(data)});

    this._hubConnection.on('Game', (data) => {this.Game(data)});

    this._hubConnection.on('Resources', (data) => {this.Resources(data)});

    this._hubConnection
      .start()
      .then(() => this.onHubConnected())
      .catch(err => console.log(err));
  }

  onHubConnected() {
  }

  SendConnected(data) {
    console.log(data);
    console.log('connected');

    this._hubConnection.invoke('SendResources', this._gameKey);
  }

  Resources(resources: IResources) {
    this._playerResources = resources.game.players;
    this._playerNum = resources.assignedNumber;
    this._dimensions = resources.game.dimensions;
    this.setConnectedStatus(true);
  }

  SendDisconnected(data) {
    console.log(data);
    console.log('disconnected');
  }

  Game(data) {
    this._playerResources[data.winner].wins += 1;
    this._matchService.runGame(data.generations, this._playerResources);
    this._hasGameArrived = true;
  }

  sendConfig() {
    this._hubConnection.invoke('SendInputConfig', this._gameKey, this._matchService.getCells());
    this._hasSent = true;
  }

  onResize(event) {
    this._matchService.resizeCanvas(event, this._playerResources);
  }

  ngAfterViewInit() {
    this.getConnectedStatus().subscribe((_connected) => {
      if (_connected) {//we need to connect to the server, get the player resources and have the view initialized before starting the UI
        this._matchService.init(this.playGrid.nativeElement, this.toolbar.nativeElement, this._dimensions);
        this._matchService.drawGrid(this._playerResources);
        this._matchService.captureEvents(this._playerResources, this._playerNum);
      }
    });
  }

  resetMatch() {
    this._matchService.resetMatch(this._playerResources);
    this._hasGameArrived = false;
    this._hasSent = false;
  }

  stopSimulation() {
    this._isPlaying = false;
    this._matchService.stopSimulation();
  }

  startSimulation() {
    this._isPlaying = true;
    this._matchService.startSimulation(this._playerResources);
  }

  skipSimulationBack() {
    this._isPlaying = false;
    this._matchService.skipSimulationBack(this._playerResources);
  }

  skipSimulationForward() {
    this._isPlaying = false;
    this._matchService.skipSimulationForward(this._playerResources);
  }

  stepSimulationBack() {
    this._isPlaying = false;
    this._matchService.stepSimulationBack(this._playerResources);
  }

  stepSimulationForward() {
    this._isPlaying = false;
    this._matchService.stepSimulationForward(this._playerResources);
  }
}
