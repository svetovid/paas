import { Component, OnInit } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';

@Component({
  selector: 'payment',
  templateUrl: './payment.component.html',
})
export class PaymentComponent implements OnInit {

  hubConnection: HubConnection | undefined;
  messages: string[] = [];
  amount: string = "";

  ngOnInit(): void {
    let builder = new HubConnectionBuilder();

    this.hubConnection = builder.withUrl("/hubs/payment").build();

    this.hubConnection.on("UpdateStatus", status => {
      this.messages.push(`Current payment status: ${status} at ${new Date()}`);
    });

    this.hubConnection.onclose(err => console.log(`Connection closed: ${err?.message} at ${new Date()}`));
    this.hubConnection.start().catch(err => console.log(err.message));
  }

  submit(): void {
    if (this.hubConnection) {
      this.hubConnection.invoke("Deposit", this.amount);
      this.amount = "";
    }
  }
}
