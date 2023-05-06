<script setup>
import axios from 'axios';

</script>

<template>
    <div class="appContainer">
        <nav>
            <div><router-link to="/">Home</router-link></div>
            <div><router-link to="/about">About</router-link></div>
            <div>Logged in as {{ username }}</div>
        </nav>
        <main>
            <router-view></router-view>
        </main>
    </div>
</template>

<style scoped>
.appContainer {
    display: flex;
    flex-direction: column;
}

nav {
    display: flex;
    column-gap: 1em;
    flex: 0 0 3em;
    align-items: center;
    background-color: antiquewhite;
    padding: 0em 1em;
}
main {
    flex-grow: 1;
    padding: 0em 1em;
    overflow-y: auto;
}
</style>
<script>
export default {
    name: 'App',
    components: {
    },
    methods: {
    },
    computed: {
        authenticated() {
            return this.$store.state.user != null;
        },
        username() {
            return this.$store.state.user?.username;
        },
    },
};
</script>





