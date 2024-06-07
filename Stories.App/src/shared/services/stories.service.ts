import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Story } from '../types/story.type';

@Injectable({
  providedIn: 'root'
})
export class StoriesService {
  apiHost = 'https://localhost:7236'
  constructor(
    private httpClient: HttpClient
  ) { }

  getStories(orderBy: string, limit: number): Observable<Story[]>
  {
    return this.httpClient.get<Story[]>(`${this.apiHost}/stories-api/Story/GetStories?OrderBy=${orderBy}&Limit=${limit}`);
  }
}
