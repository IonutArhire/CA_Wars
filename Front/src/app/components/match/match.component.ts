import { Component, ViewChild, ElementRef } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr';
import { ActivatedRoute, Router } from '@angular/router';

import { Observable } from 'rxjs';
import { BehaviorSubject } from 'rxjs';
import 'rxjs/add/observable/of'

import { MatchService } from '../../services/match.service';

import { IPlayerResource } from '../../resources/player-resources';
import { IDimensionsResources } from '../../resources/dimensions-resources';
import { IMatchResources } from '../../resources/match-resources';

import { Guid } from 'guid-typescript';



@Component({
  selector: 'app-match',
  templateUrl: './match.component.html',
  styleUrls: ['./match.component.css']
})
export class MatchComponent {

  private _hubConnection: HubConnection

  @ViewChild('playGrid') playGrid: ElementRef;
  @ViewChild('toolbar') toolbar: ElementRef;

  @ViewChild("side") side: any;

  private _resourcesStatus: BehaviorSubject<boolean>;
  
  private _dimensions: IDimensionsResources;
  
  private _playerResources: Array<IPlayerResource>;
  private _assignedNum: number;
  
  private _matchKey: Guid;
  
  private _map: number[][];

  private _eraserMode: boolean;
  private _gridMode: boolean;

  private _isMatchKeyValid: boolean;

  private _errorMessage: string;

  private _playersWhoSent: Array<number>;

  public _hasResources: boolean;
  public _isPlaying: boolean;
  public _hasGameArrived: boolean;
  public _hasSent: boolean;

  constructor(private _matchService: MatchService, 
              private _route: ActivatedRoute,
              private _router: Router) {
                
    this._hasResources = false;
    this._resourcesStatus = new BehaviorSubject<boolean>(false);
    
    this._assignedNum = -1;

    this._isPlaying = false;
    this._hasGameArrived = false;
    this._hasSent = false;

    this._eraserMode = false;
    this._gridMode = true;

    this._isMatchKeyValid = true;

    this._playersWhoSent = new Array<number>();
  }

  ngOnInit() {
    let unparsedMatchKey = this._route.snapshot.paramMap.get('match-key');

    if (Guid.isGuid(unparsedMatchKey)) {
      this._matchKey = Guid.parse(unparsedMatchKey);
    }
    else {
      this._errorMessage = `\"${unparsedMatchKey}\" is not a valid match key!`;
      this._isMatchKeyValid = false;
      return;
    }

    this._hubConnection = new HubConnectionBuilder()
                              .withUrl('http://localhost:5000/match')
                              .build();
                            
    this._hubConnection.serverTimeoutInMilliseconds = 10000 * 1000;

    this._hubConnection.on('Connected', (data) => {this.connected(data)});

    this._hubConnection.on('Disconnected', (data) => {this.disconnected(data)});

    this._hubConnection.on('Game', (data) => {this.game(data)});

    this._hubConnection.on('PlayerSent', (data) => {this.playerSent(data)});

    this._hubConnection.on('Resources', (data) => {this.resources(data)});

    this._hubConnection
      .start()
      .then(() => {})
      .catch();
  }

  public getResourcesStatus(): Observable<boolean> {
    return this._resourcesStatus.asObservable();
  }

  public setResourcesStatus(newValue: boolean): void {
    this._hasResources = newValue;
    this._resourcesStatus.next(newValue);
  }

  public connected(data): void {
    this._hubConnection.invoke('SendResources', this._matchKey.toString())
      .catch((err) => {
        this._errorMessage = err.message.match("HubException: (.*)")[1];
        this._isMatchKeyValid = false;
      });
  }
  
  public resources(resources: IMatchResources): void {
    this._playerResources = resources.players;
    this._assignedNum = resources.assignedNumber;
    this._dimensions = resources.dimensions;
    this._map = resources.map;
    this.setResourcesStatus(true);
  }

  public disconnected(data): void {
  }

  public game(data): void {
    this._playersWhoSent = new Array<number>();
    if (data.winner != -1) {
      this._playerResources[data.winner].wins += 1;
    }
    this._matchService.updatePlayerResources(this._playerResources);
    this._matchService.initSimulation(data.generations);
    this._hasGameArrived = true;
  }

  public sendConfig(): void {
    this._hubConnection.invoke('SendConfig', this._matchKey.toString(), this._matchService.getCells(), this._assignedNum);
    this._hasSent = true;
  }

  public playerSent(assignedNumber): void {
    this._playersWhoSent.push(assignedNumber);
  }

  public ngAfterViewInit(): void {
    this.getResourcesStatus().subscribe((_gotResources) => {
      if (_gotResources) {//we need to connect to the server, get the player resources and have the view initialized before starting the UI
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
    if (this._eraserMode) {
      this._matchService.deactivateEraserMode();
    }
    else {
      this._matchService.activateEraserMode();
    }

    this._eraserMode = !this._eraserMode;
  }

  public flipGridMode(): void {
    if (this._gridMode) {
      this._matchService.deactivateGridMode();
    }
    else {
      this._matchService.activateGridMode();
    }

    this._gridMode = !this._gridMode;
  }

  public downloadGenerations(): void {
    this._matchService.saveGenerationsAsImgs();
  }

  public exit(): void {
    this.side.show();
  }

  public exitMatch(): void {
    this._hubConnection.stop();
    this._router.navigateByUrl('menu/main');
  }
}
