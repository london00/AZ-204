<template>
    <div class="container">
        <h3>Recent orders</h3>

        <div class="row" v-if="!ready">
            <div class="col-sm">
                <div>Loading...</div>
            </div>
        </div>
        <div v-if="ready">
            <div class="row" v-for="(order) in orders" :key="order.customerName">
                <div class="col-sm">
                    <hr />
                    <div>
                        <div style="display: inline-block; padding-left: 12px;">
                            <div>
                                <span class="text-info small"><strong>{{ order.customerName }}</strong> just ordered a</span>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
    import { useSignalR } from '@dreamonkey/vue-signalr';

    export default {
        name: 'HttpSignalRExampleComponent',
        props: {
            //msg: String
        },
        data() {
            return {
                ready: false,
                orders: []
            }
        },
        methods: {
            productOrdered(orderPlacement) {
                this.orders.push(orderPlacement);
            },
            setup() {
                const signalr = useSignalR();

                //signalr.invoke('SendMessage', { message });
                signalr.on('orderCreated', this.productOrdered);

                this.ready = true;
            },
        },
        created() {
            this.setup();
        }
    }
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
</style>
