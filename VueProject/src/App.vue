<template>
    <MainComponent msg="My fist Vue.js App" />
    <HttpSignalRExampleComponent />
</template>

<script>
    import MainComponent from './components/MainComponent.vue'
    //import HttpPolingExampleComponent from './components/HttpPolingExampleComponent.vue'
    import HttpSignalRExampleComponent from './components/HttpSignalRExampleComponent.vue'

    export default {
        name: 'App',
        components: {
            MainComponent,
            HttpSignalRExampleComponent
        },
    }
</script>

<style>
    #app {
        font-family: Avenir, Helvetica, Arial, sans-serif;
        -webkit-font-smoothing: antialiased;
        -moz-osx-font-smoothing: grayscale;
        text-align: center;
        color: #2c3e50;
        margin-top: 60px;
    }
</style>
