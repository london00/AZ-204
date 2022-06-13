<template>
    <ul>
        <li v-for="(person) in people" :key="person.id">
            {{ person.name }}
        </li>
    </ul>
</template>

<script>
    import axios from "axios";
    const LOCAL_BASE_URL = 'http://localhost';

    export default {
        name: 'HttpPolingExampleComponent',
        props: {
            //msg: String
        },
        interval: null,
        data() {
            return {
                people: []
            }
        },
        methods: {
            async update()
            {
                try
                {
                    const apiUrl = `${LOCAL_BASE_URL}/api/v1/people`;
                    const response = await axios.get(apiUrl);

                    this.people = response.data;
                }
                catch (ex) {
                    console.error(ex);
                }
            },
            startPoll() {
                this.interval = setInterval(this.update, 5000);
            }
        },
        created() {
            this.update();
            this.startPoll();
        }
    }
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
</style>
