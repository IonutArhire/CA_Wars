import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http'
import { Observable } from 'rxjs';
import { MatchCreate } from '../entities/match-create-model';
import { TouchSequence } from 'selenium-webdriver';


@Injectable()
export class MatchCreateService {

	private matchCreateUrl = 'http://localhost:5000/api/v1/match/create';

	private rulesets: Array<string>;

	constructor(private http: HttpClient) {
		this.rulesets = [
		"WalledCities",
		"PseudoLife",
		"Assimilation",
		"34 Life",
		"Diamoeba",
		"HighLife",
		"Replicator",
		"2x2",
		"Maze",
		"Day & Night",
		"Seeds (2)",
		"Coral",
		"Coagulations",
		"Stains",
		"Gnarl",
		"GOF",
		"Serviettes",
		"LongLife",
		"Move",
		"Mazectric",
		"Flakes"]; 
	}

	public getRulesets() {
		return this.rulesets;
	}

	public createMatch (matchCreateModel: MatchCreate): Observable<object> {
		return this.http.post<object>(this.matchCreateUrl, matchCreateModel);
	}

}
