import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';

import { AppComponent } from './app.component';
import { MatchService } from './services/match.service';
import { AppRoutingModule } from './app-routing.module';
import { MatchComponent } from './components/match/match.component';
import { MatchCreateComponent } from './components/match-create/match-create.component';
import { MainMenuComponent } from './components/main-menu/main-menu.component';
import { MenuComponent } from './components/menu/menu.component';
import { HttpClientModule } from '@angular/common/http';
import { MatchCreateService } from './services/match-create.service';
import { MDBBootstrapModule } from 'angular-bootstrap-md';
import { ErrorComponent } from './components/error/error.component';

@NgModule({
  declarations: [
    AppComponent,
    MatchComponent,
    MatchCreateComponent,
    MainMenuComponent,
    MenuComponent,
    ErrorComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    MDBBootstrapModule.forRoot()
  ],
  providers: [MatchService, MatchCreateService],
  bootstrap: [AppComponent]
})
export class AppModule { }
