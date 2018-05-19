import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { 
  Idea,
  Like
} from '../models';

@Injectable()
export class IdeaService {

  private readonly apiUrl = 'api/idea'; 

  constructor(private http: HttpClient) { }
  
  getIdeas(filterId=0, periodId=3, tag=null, lastIdeaId=null): Observable<Idea[]> {
    let params = new HttpParams();
    params = params.append('filter', filterId.toString());
    params = params.append('period', periodId.toString());
    if (tag)
      params = params.append('tagId', tag.id);
    if (lastIdeaId)
      params = params.append('lastIdeaId', lastIdeaId);

    return this.http.get<Idea[]>(`${this.apiUrl}/getlist`, { params: params });
  }

  searchIdeas(searchValue, lastIdeaId=null): Observable<Idea[]> {
    let params = new HttpParams();
    params = params.append('searchValue', searchValue);
    if (lastIdeaId)
      params = params.append('lastIdeaId', lastIdeaId);

    return this.http.get<Idea[]>(`${this.apiUrl}/searchlist`, { params: params });
  }

  getIdeaDetail(id): Observable<Idea> {
    let params = new HttpParams();
    params = params.append('ideaId', id);

    return this.http.get<Idea>(`${this.apiUrl}/getdetail`, { params: params });
  }

  createOrUpdate(newIdea) {
    return this.http.post<any>(`${this.apiUrl}/createorupdate`, newIdea);
  }

  like(data: Like) {
    return this.http.post<any>(`${this.apiUrl}/like`, data);
  }

  setFavorite(id) {
    let params = new HttpParams();
    params = params.append('ideaId', id);

    return this.http.post<any>(`${this.apiUrl}/addtofavorites`, params);
  }

  removeOrRestore(id) {
    let params = new HttpParams();
    params = params.append('ideaId', id);

    return this.http.post<any>(`${this.apiUrl}/removeorrestore`, params);
  }
}
