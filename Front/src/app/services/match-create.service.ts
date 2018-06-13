import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http'
import { Observable } from 'rxjs';
import { MatchCreate } from '../entities/match-create-model';


@Injectable()
export class MatchCreateService {

	private matchCreateUrl = 'http://localhost:5000/api/v1/match/create';

	constructor(private http: HttpClient) {

	}

	createMatch (matchCreateModel: MatchCreate): Observable<string> {
		return this.http.post<string>(this.matchCreateUrl, matchCreateModel);
  }

}
