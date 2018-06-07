import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { GameComponent } from './components/game/game.component';
import { MatchCreateComponent } from './components/match-create/match-create.component';

const routes: Routes = [
  { path: 'match/create', component: MatchCreateComponent },
  { path: 'match/:game-key', component: GameComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }