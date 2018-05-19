import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { 
  Idea, 
  Profile,
  PopularUser,
  UserSettings
} from '../models';

@Injectable()
export class ProfileService {

  private readonly apiUrl = 'api/profile';
  
  constructor(private http: HttpClient) { }

  getInfo(userName): Observable<Profile> {
    let params = new HttpParams();
    params = params.append('userName', userName);

    return this.http.get<Profile>(`${this.apiUrl}/getinfo`, { params: params });
  }

  getIdeaList(kindId=0, userName, lastIdeaId=null): Observable<Idea[]> {
    let params = new HttpParams();
    params = params.append('kind', kindId.toString());
    params = params.append('userName', userName);
    if (lastIdeaId)
      params = params.append('lastIdeaId', lastIdeaId);

    return this.http.get<Idea[]>(`${this.apiUrl}/getidealist`, { params: params });
  }

  getPopularUsers() : Observable<PopularUser[]> {
    return this.http.get<PopularUser[]>(`${this.apiUrl}/getpopularusers`);
  }

  getUserSettings() : Observable<UserSettings> {
    return this.http.post<UserSettings>(`${this.apiUrl}/getusersettings`, null);
  }

  updateUserSettings(userSettings: UserSettings) {
    return this.http.post<any>(`${this.apiUrl}/updateusersettings`, userSettings);
  }

}
