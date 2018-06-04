import { Component, ViewChild, ElementRef, HostListener } from '@angular/core';
import { HubConnection } from '@aspnet/signalr';
import { ActivatedRoute, Event } from '@angular/router';

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

  private _hasResources: boolean;
  private _resourcesStatus: BehaviorSubject<boolean>;

  private _dimensions: IDimensionsResources;

  private _playerResources: Array<IPlayerResource>;
  private _assignedNum: number;

  private _gameKey: number;

  private _isPlaying: boolean;
  private _hasGameArrived: boolean;
  private _hasSent: boolean;


  constructor(private _matchService: MatchService, private _route: ActivatedRoute) {
    this._hasResources = false;
    this._resourcesStatus = new BehaviorSubject<boolean>(false);
    
    this._assignedNum = -1;
    this._gameKey = -1;

    this._isPlaying = false;
    this._hasGameArrived = false;
    this._hasSent = false;
  }

  ngOnInit() {
    this._gameKey = parseInt(this._route.snapshot.paramMap.get('game-key'));

    this._hubConnection = new HubConnection('http://localhost:5000/match');

    this._hubConnection.on('Connected', (data) => {this.connected(data)});

    this._hubConnection.on('Disconnected', (data) => {this.disconnected(data)});

    this._hubConnection.on('Game', (data) => {this.game(data)});

    this._hubConnection.on('Resources', (data) => {this.resources(data)});

    this._hubConnection
      .start()
      .then(() => {})
      .catch(err => console.log(err));
  }

  public getResourcesStatus(): Observable<boolean> {
    return this._resourcesStatus.asObservable();
  }

  public setResourcesStatus(newValue: boolean): void {
    this._hasResources = newValue;
    this._resourcesStatus.next(newValue);
  }

  public connected(data): void {
    console.log(data);
    console.log('connected');

    this._hubConnection.invoke('SendResources', this._gameKey);
  }

  public resources(resources: IResources): void {
    this._playerResources = resources.game.players;
    this._assignedNum = resources.assignedNumber;
    this._dimensions = resources.game.dimensions;
    this.setResourcesStatus(true);
  }

  public disconnected(data): void {
    console.log(data);
    console.log('disconnected');
  }

  public game(data): void {
    this._playerResources[data.winner].wins += 1;
    this._matchService.updatePlayerResources(this._playerResources);
    this._matchService.runSimulation(data.generations);
    this._hasGameArrived = true;
  }

  public sendConfig(): void {
    this._hubConnection.invoke('SendConfig', this._gameKey, this._matchService.getCells());
    this._hasSent = true;
  }

  public ngAfterViewInit(): void {
    this.getResourcesStatus().subscribe((_connected) => {
      if (_connected) {//we need to connect to the server, get the player resources and have the view initialized before starting the UI
        this._matchService.init(this.playGrid.nativeElement, this.toolbar.nativeElement, this._dimensions, this._playerResources);
        this._matchService.drawGrid();
        this._matchService.captureEvents(this._assignedNum);
      }
    });
  }

  public onResize(event): void {
    this._matchService.resizeCanvas(event.target.innerWidth, event.target.innerHeight);
  }

  public resetMatch(): void {
    this._matchService.resetMatch();
    this._hasGameArrived = false;
    this._hasSent = false;
  }

  public stopSimulation(): void {
    this._isPlaying = false;
    this._matchService.stopSimulation();
  }

  public startSimulation(): void {
    this._isPlaying = true;
    this._matchService.startSimulation();
  }

  public skipSimulationBack(): void {
    this._isPlaying = false;
    this._matchService.skipSimulationBack();
  }

  public skipSimulationForward(): void {
    this._isPlaying = false;
    this._matchService.skipSimulationForward();
  }

  public stepSimulationBack(): void {
    this._isPlaying = false;
    this._matchService.stepSimulationBack();
  }

  public stepSimulationForward(): void {
    this._isPlaying = false;
    this._matchService.stepSimulationForward();
  }
}
