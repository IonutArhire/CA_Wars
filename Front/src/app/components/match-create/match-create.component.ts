import { Component } from '@angular/core';
import { MatchCreateService } from '../../services/match-create.service';
import { MatchCreate } from '../../entities/match-create-model';

@Component({
  selector: 'app-match-create',
  templateUrl: './match-create.component.html',
  styleUrls: ['./match-create.component.css']
})
export class MatchCreateComponent {

  constructor(private _matchCreateService: MatchCreateService) { }

  ngOnInit() {
  }

  ngAfterViewInit() {
    this._matchCreateService.createMatch(new MatchCreate(2, "GOF", 200, 30, 30))
      .subscribe(result => console.log(result));
  }

}
