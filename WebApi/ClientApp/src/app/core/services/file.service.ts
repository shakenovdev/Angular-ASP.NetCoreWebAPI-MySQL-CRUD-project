import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable()
export class FileService {

  private readonly apiUrl = "api/file";

  constructor(private http: HttpClient) { }

  upload(file: File) {
    let formData:FormData = new FormData();
    formData.append('File', file, file.name);
    let headers = new Headers();

    return this.http.post<any>(`${this.apiUrl}/upload`, formData);
  }
}
