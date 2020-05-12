<template>
  <v-app id = 'app'>
    <div id="static">
    <div id="nav">
      <router-link to="/home">Home</router-link> |
      <router-link to="/about">About</router-link> |
      <router-link to="/inbox">Inbox</router-link> |
      <router-link to="/search">Search</router-link> |
      <router-link to="/registration">Registration</router-link> |
      <router-link to="/householdCreation">HouseholdCreation</router-link>
    </div>
    <router-view/>
  </div>
  </v-app>
</template>

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
