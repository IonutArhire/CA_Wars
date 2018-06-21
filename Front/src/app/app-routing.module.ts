import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { MatchComponent } from './components/match/match.component';
import { MatchCreateComponent } from './components/match-create/match-create.component';
import { MainMenuComponent } from './components/main-menu/main-menu.component';
import { MenuComponent } from './components/menu/menu.component';
import { AboutComponent } from './components/about/about.component';
import { HelpComponent } from './components/help/help.component';


const routes: Routes = [
  { path: '', redirectTo: 'menu/main', pathMatch: 'full' },
  { path: 'menu', 
    component: MenuComponent,
    children: [
      { path: 'main', component: MainMenuComponent },
      { path: 'multiplayer/create', component: MatchCreateComponent },
      { path: 'about', component: AboutComponent },
      { path: 'help', component: HelpComponent }
    ]
  },
  { path: 'match/:match-key', component: MatchComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }