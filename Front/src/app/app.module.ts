import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppComponent } from './app.component';
import { MatchService } from './services/match.service';
import { AppRoutingModule } from './app-routing.module';
import { GameComponent } from './components/game/game.component';

@NgModule({
  declarations: [
    AppComponent,
    GameComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule
  ],
  providers: [MatchService],
  bootstrap: [AppComponent]
})
export class AppModule { }
