<template>
  <v-app id = 'app'>
    <div id="static">
    <div id="nav">
      <div v-if="isLoggedIn()">
        <router-link to="/home">Home</router-link> |
        <router-link to="/about">About</router-link> |
        <router-link to="/inbox">Inbox</router-link> |
        <router-link to="/search">Search</router-link>
      </div>
      <div v-if="!isLoggedIn()">
        <router-link to="/login">Home</router-link> |
        <router-link to="/about">About</router-link>
      </div>
    </div>
    <router-view/>
    </div>
  </v-app>
</template>

<script>
export default {
  methods: {
    isLoggedIn () {
      if (this.$store.getters.isauthenticated) {
        return true
      } else {
        return false
      }
    }
  }
}
</script>

<style>
#app {
  font-family: "Helvetica Neue", Helvetica;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
}

#nav {
  padding: 30px;
}

#nav a {
  font-weight: bold;
  color: #2c3e50;
}

#nav a.router-link-active {
  color: #22b573;
}
</style>
