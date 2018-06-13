import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';

import { AppComponent } from './app.component';
import { MatchService } from './services/match.service';
import { AppRoutingModule } from './app-routing.module';
import { GameComponent } from './components/game/game.component';
import { MatchCreateComponent } from './components/match-create/match-create.component';
import { MainMenuComponent } from './components/main-menu/main-menu.component';
import { MenuComponent } from './components/menu/menu.component';
import { HttpClientModule } from '@angular/common/http';
import { MatchCreateService } from './services/match-create.service';

@NgModule({
  declarations: [
    AppComponent,
    GameComponent,
    MatchCreateComponent,
    MainMenuComponent,
    MenuComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule
  ],
  providers: [MatchService, MatchCreateService],
  bootstrap: [AppComponent]
})
export class AppModule { }
