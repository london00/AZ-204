import { createApp } from 'vue'
import App from './App.vue'
import { VueSignalR } from '@dreamonkey/vue-signalr';
import { HubConnectionBuilder } from '@microsoft/signalr';

const connection = new HubConnectionBuilder()
    .withUrl('http://localhost/api/v1')
    .build();

createApp(App).use(VueSignalR, { connection }).mount('#app');