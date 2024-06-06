import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class StoriesService {
  apiHost = 'https://localhost:7236'
  constructor(
    private httpClient: HttpClient
  ) { }

  getStories(orderBy: string, limit: number): Observable<string>
  {
    return this.httpClient.get<string>(`${this.apiHost}/stories-api/Story/GetStories?OrderBy=${orderBy}&Limit=${limit}`);
  }
}
