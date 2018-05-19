import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { Tag } from '../models';

@Injectable()
export class TagService {

  private readonly apiUrl = 'api/tag';

  constructor(private http: HttpClient) { }

  getPopular(): Observable<Tag[]> {
    return this.http.get<Tag[]>(`${this.apiUrl}/getpopular`);
  }

}
