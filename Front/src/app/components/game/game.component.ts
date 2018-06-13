import { Component, ViewChild, ElementRef } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr';
import { ActivatedRoute } from '@angular/router';

import { Observable } from 'rxjs';
import { BehaviorSubject } from 'rxjs';
import 'rxjs/add/observable/of'

import { MatchService } from '../../services/match.service';

import { IPlayerResource } from '../../resources/player-resources';
import { IDimensionsResources } from '../../resources/dimensions-resources';
import { IGameResources } from '../../resources/game-resources';

import { Guid } from 'guid-typescript';



@Component({
  selector: 'app-game',
  templateUrl: './game.component.html',
  styleUrls: ['./game.component.css']
})
export class GameComponent {

  private _hubConnection: HubConnection

  @ViewChild('playGrid') playGrid: ElementRef;
  @ViewChild('toolbar') toolbar: ElementRef;
  @ViewChild('eraser') eraser: ElementRef;
  @ViewChild('gridMode') gridMode: ElementRef;

  private _resourcesStatus: BehaviorSubject<boolean>;
  
  private _dimensions: IDimensionsResources;
  
  private _playerResources: Array<IPlayerResource>;
  private _assignedNum: number;
  
  private _gameKey: Guid;
  
  private _map: number[][];

  public _hasResources: boolean;
  public _isPlaying: boolean;
  public _hasGameArrived: boolean;
  public _hasSent: boolean;

  constructor(private _matchService: MatchService, private _route: ActivatedRoute) {
    this._hasResources = false;
    this._resourcesStatus = new BehaviorSubject<boolean>(false);
    
    this._assignedNum = -1;
    this._gameKey = Guid.createEmpty();

    this._isPlaying = false;
    this._hasGameArrived = false;
    this._hasSent = false;
  }

  ngOnInit() {
    this._gameKey = Guid.parse(this._route.snapshot.paramMap.get('game-key'));
    

    this._hubConnection = new HubConnectionBuilder()
                              .withUrl('http://localhost:5000/match')
                              .build();
                            
    this._hubConnection.serverTimeoutInMilliseconds = 10000 * 1000;

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

    this._hubConnection.invoke('SendResources', this._gameKey.toString()).catch(this.resourcesError);
  }

  public resourcesError(err): void {
    console.log(err);
  }

  public resources(resources: IGameResources): void {
    this._playerResources = resources.players;
    this._assignedNum = resources.assignedNumber;
    this._dimensions = resources.dimensions;
    this._map = resources.map;
    this.setResourcesStatus(true);
  }

  public disconnected(data): void {
    console.log(data);
    console.log('disconnected');
  }

  public game(data): void {
    this._playerResources[data.winner].wins += 1;
    this._matchService.updatePlayerResources(this._playerResources);
    this._matchService.initSimulation(data.generations);
    this._hasGameArrived = true;
  }

  public sendConfig(): void {
    this._hubConnection.invoke('SendConfig', this._gameKey, this._matchService.getCells());
    this._hasSent = true;
  }

  public ngAfterViewInit(): void {
    this.getResourcesStatus().subscribe((_connected) => {
      if (_connected) {//we need to connect to the server, get the player resources and have the view initialized before starting the UI
        this._matchService.init(this.playGrid.nativeElement, this.toolbar.nativeElement, this._dimensions, this._playerResources, this._map);
        this._matchService.drawGrid();
        this._matchService.captureEvents(this._assignedNum);
      }
    });
  }

  public onResize(event): void { //TODO: type of event
    this._matchService.resizeCanvas(event.target.innerWidth, event.target.innerHeight);
  }

  public resetMatch(): void {
    this._matchService.resetMatch();
    this._isPlaying = false;
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

  public flipEraserMode(): void {
    let eraserMode = this._matchService.getEraserMode();

    if (eraserMode) {
      this._matchService.deactivateEraserMode();
      (<HTMLElement>this.eraser.nativeElement).style.color = "#616161"; //grey
    }
    else {
      this._matchService.activateEraserMode();
      (<HTMLElement>this.eraser.nativeElement).style.color = "#f28910"; //orange
    }
  }

  public flipGridMode(): void {
    let gridMode = this._matchService.getGridMode();

    if (gridMode) {
      this._matchService.deactivateGridMode();
      (<HTMLElement>this.gridMode.nativeElement).style.color = "#dadada5b";
    }
    else {
      this._matchService.activateGridMode();
      (<HTMLElement>this.gridMode.nativeElement).style.color = "#616161";
    }
  }

  public downloadGenerations(): void {
    this._matchService.saveGenerationsAsImgs();
  }
}
