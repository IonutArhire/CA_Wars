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
		this.rulesets = ["GOF", 
										"Coagulations"]; 
	}

	public getRulesets() {
		return this.rulesets;
	}

	public createMatch (matchCreateModel: MatchCreate): Observable<string> {
		return this.http.post<string>(this.matchCreateUrl, matchCreateModel);
	}

}
