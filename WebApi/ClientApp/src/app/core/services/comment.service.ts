import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { 
  Comment,
  Like 
} from '../models';

@Injectable()
export class CommentService {

  private readonly apiUrl = 'api/comment';

  constructor(private http: HttpClient) { }

  createOrUpdate(newComment) {
    return this.http.post<any>(`${this.apiUrl}/createorupdate`, newComment);
  }

  getList(ideaId, creatorId) {
    let params = new HttpParams();
    params = params.append('ideaId', ideaId);

    return this.http.get<Comment[]>(`${this.apiUrl}/getlist`, { params: params })
      .map(result => {
        result.forEach(comment => comment.isOP = (comment.creator.id === creatorId));
        return result;
      });
  }

  like(data: Like) {
    return this.http.post<any>(`${this.apiUrl}/like`, data);
  }

  removeOrRestore(id) {
    let params = new HttpParams();
    params = params.append('id', id);

    return this.http.post<any>(`${this.apiUrl}/removeorrestore`, params);
  }
}
