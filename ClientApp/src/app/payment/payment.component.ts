import { Component, OnInit } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr';

@Component({
  selector: 'payment',
  templateUrl: './payment.component.html',
})
export class PaymentComponent implements OnInit {

  public hubConnection: HubConnection;
  public messages: string[] = [];
  public amount: string;

  ngOnInit(): void {
    let builder = new HubConnectionBuilder();
    
    this.hubConnection = builder.withUrl("/hubs/payment").build();

    this.hubConnection.on("UpdateStatus", status => {
      alert(status);
      this.messages.push(`Current payment status: ${status} at ${new Date()}`);
    });

    this.hubConnection.onclose(err => this.messages.push(`Connection closed: ${err.message} at ${new Date()}`));
    //this.hubConnection.serverTimeoutInMilliseconds = 300000;
    this.hubConnection.start();
  }

  submit(): void {
    this.hubConnection.invoke("Deposit", this.amount);
    this.amount = "";
  }
}
