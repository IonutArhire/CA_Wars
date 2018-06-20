import { Component, ViewChild  } from '@angular/core';
import { MatchCreateService } from '../../services/match-create.service';
import { MatchCreate } from '../../entities/match-create-model';
import { isNumeric } from 'rxjs/util/isNumeric';
import { Router } from '@angular/router';

@Component({
  selector: 'app-match-create',
  templateUrl: './match-create.component.html',
  styleUrls: ['./match-create.component.css']
})
export class MatchCreateComponent {

  @ViewChild("side") side: any;

  private _selectedNrPlayers: number;
  private _selectedRuleSet: string;
  private _rulesets: Array<string>;

  private _maxIters: number;
  private _rows: number;
  private _cols: number;

  private _maxItersError: number;
  private _rowsError: number;
  private _colsError: number;

  private _isReadyToCreate: boolean;

  private _errors: { [errorCode: number] : string; };

  private _urlRoot = "http://localhost:4200/match/";

  private _matchId: string;
  private _matchUrl: string;

  constructor(private _matchCreateService: MatchCreateService,
              private _router: Router) {

    this._selectedNrPlayers = 2;
    this._selectedRuleSet = "GOF";

    this._maxItersError = -1;
    this._rowsError = -1;
    this._colsError = -1;

    this._isReadyToCreate = false;

    this._errors = {
      1: "Input is not a number",
      2: "Number isn't in the required range for this input"
    };
  }
  
  ngOnInit() {
    this._rulesets = this._matchCreateService.getRulesets();
  }

  ngAfterViewInit() {
    
  }

  private updateCreateStatus(): void {
    if (this._maxItersError == 0 && this._rowsError == 0 && this._colsError == 0) {
      this._isReadyToCreate = true;
    }
    else {
      this._isReadyToCreate = false;
    }
  }

  private updateErrorVars(elemName: string, statusCode: number): void {
    if (elemName === "MaxIters") {
      this._maxItersError = statusCode;
    }
    else if (elemName === "Rows") {
      this._rowsError = statusCode;
    }
    else if (elemName === "Cols") {
      this._colsError = statusCode;
    }

    this.updateCreateStatus();
  }

  private onChangeNumericInput(val: any, elemName: string, infLimit: number, supLimit: number): void {
    if (val == "") {
      this.updateErrorVars(elemName, 0);
      return;
    }

    if (isNumeric(val)) {
      let num = Number(val);

      if (num >= infLimit && num <= supLimit) {
        this.updateErrorVars(elemName, 0);
      }
      else {
        this.updateErrorVars(elemName, 2);
      }
    }
    else {
      this.updateErrorVars(elemName, 1);
    }
  }

  private createMatch(content): void {
    let matchCreateObj = new MatchCreate(this._selectedNrPlayers, this._selectedRuleSet, this._maxIters, this._rows, this._cols);
    this._matchCreateService.createMatch(matchCreateObj)
      .subscribe((result) => {
        this._matchId = result["id"];
        this._matchUrl = `${this._urlRoot}${this._matchId}`;

        this.side.show();
      });
  }

  private goToMatch(): void {
    this._router.navigateByUrl('match/' + this._matchId);
  }

}
