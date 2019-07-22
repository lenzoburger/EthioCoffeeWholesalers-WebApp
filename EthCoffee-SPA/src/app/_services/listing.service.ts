import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Observable, throwError } from 'rxjs';
import { Listing } from '../_models/listing';
import { HttpClient, HttpParams } from '@angular/common/http';
import { AuthService } from './auth.service';
import { map, catchError } from 'rxjs/operators';
import { PaginatedResult } from '../_models/pagination';

@Injectable({
  providedIn: 'root'
})
export class ListingService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient, private authService: AuthService) {}

  getListing(id): Observable<Listing> {
    return this.http.get<Listing>(this.baseUrl + 'listings/' + id);
  }

  getListings(pageNumber?, pageSize?): Observable<PaginatedResult<Listing[]>> {
    const paginatedResult: PaginatedResult<Listing[]> = new PaginatedResult<Listing[]>();
    let params = new HttpParams();
    if (pageNumber != null) {
      params = params.append('pageNumber', pageNumber);
    }

    if (pageSize != null ) {
      params = params.append('pageSize', pageSize);
    }

    return this.http.get<Listing[]>(this.baseUrl + 'listings', { observe: 'response', params }).pipe(
      map( response => {
        paginatedResult.result = response.body;
        if (response.headers.get('Pagination') != null) {
          paginatedResult.pagination = JSON.parse(response.headers.get('Pagination'));
        }
        return paginatedResult;
      })
    );
  }

  getListingForEdit(id): Observable<Listing> {
    if (this.authService.loggedIn()) {
      return this.getListing(id).pipe(
        map(listing => {
          if ('' + listing.user.id === this.authService.decodedToken.nameid) {
            return listing;
          } else {
            throw new Error();
          }
        }),
        catchError(error => {
          return throwError('Login required to perform this action');
        })
      );
    } else {
      return throwError('Please login.');
    }
  }

  updateListing(id: number, listing: Listing) {
    return this.http.put(this.baseUrl + 'listings/' + id, listing);
  }

  setMainPhoto(listingId: number, id: number) {
    return this.http.post(
      this.baseUrl + 'listings/' + listingId + '/photos/' + id + '/setMain',
      {}
    );
  }

  deletePhoto(listingId: number, id: number) {
    return this.http.delete(
      this.baseUrl + 'listings/' + listingId + '/photos/' + id
    );
  }
}
