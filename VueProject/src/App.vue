<template>
    <MainComponent msg="My fist Vue.js App" />
    <HttpPolingExampleComponent />
</template>

<script>
    import MainComponent from './components/MainComponent.vue'
    import HttpPolingExampleComponent from './components/HttpPolingExampleComponent.vue'

    export default {
        name: 'App',
        components: {
            MainComponent,
            HttpPolingExampleComponent
        }
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
