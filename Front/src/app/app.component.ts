import { Component } from '@angular/core';
import { HubConnection } from '@aspnet/signalr';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {

  ngOnInit() {
    let hubConnection = new HubConnection('http://localhost:5000/match');

    hubConnection.on('SendAction', data => {
      console.log(data);
    });

    hubConnection
      .start()
      .then(() => console.log('connection successful!'))
      .catch(err => console.log(err));
  }
  
}
