<!-- <template>
  <div class="login">
    <div class="logo">
      <img alt="RoomAid" src="../assets/Room-Aid-Logo_Final.png">
    </div>
  </div>
</template> -->
<template>
  <div>
    <div class="logo">
      <img alt="RoomAid" src="../assets/Room-Aid-Logo_Final.png">
    </div>
    <v-content>
      <v-alert id="login-error" :value=showError>
          <span>
            {{ errors }}
          </span>
      </v-alert>
      <v-layout row justify-center>
        <v-container id="login" justify="center">
          <v-card>
            <v-toolbar dark color="deep-purple darken-4">
              <v-toolbar-title>LOGIN</v-toolbar-title>
            </v-toolbar>
            <div id="fields-div">
              <v-form ref="form" v-model="valid">
                <v-text-field v-model="email"
                  label="Email"
                  name = "email"
                  :disabled=disable
                  required
                  solo
                ></v-text-field>
                <v-text-field v-model="password"
                  name="password"
                  label="Password"
                  id="password"
                  :min="8"
                  :type=" visible ? 'text' : 'password'"
                  :disable=disable
                  required
                  solo
                  ></v-text-field>
                <v-btn color="success" @click="LoginUser" :disabled="!valid">Sign In</v-btn>
                <!-- <div class="text-right">
                  <router-link class="md-accent" to="ResetPassword">Forgot password?</router-link>
                </div>
                <div class="text-right">
                  <router-link class="md-accent" to="/Registration">Don't have an account?</router-link>
                </div> -->
              </v-form>
            </div>
          </v-card>
        </v-container>
      </v-layout>
    </v-content>
  </div>
</template>

<script>
// @ is an alias to /src
// import HelloWorld from '@/components/HelloWorld.vue'

export default {
  name: 'Login',
  components: {
  },
  data: () => ({
    email: '',
    password: '',
    visible: false,
    valid: false,
    disable: false,
    showError: false,
    errors: null,
    decodedToken: null
  }),
  beforeCreate () {
    try {
      if (this.$store.getters.authenticationtoken !== null) {
        this.$router.push('/home')
      }
    } catch (ex) {
      console.log(ex)
    }
  },
  methods: {
    // POST REQUEST
    LoginUser (username, password) {
      this.showError = false
      this.valid = false
      this.disable = true
      const uri = `${this.$hostname}/api/login/loginaccount`
      const req = new Request(uri, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        mode: 'cors',
        body: JSON.stringify({ Email: this.email, Password: this.password })
      })
      fetch(req)
        .then(response => {
          if (response.ok) {
            return response.json()
          } else {
            throw new Error(response.status)
          }
        })
        .then(data => {
          this.$store.dispatch('updateAuthenticationStatus', true)
          this.$store.dispatch('updateAuthenticationToken', data)
          this.decodedToken = jwt_decode(data)
          this.$store.dispatch('updateUserId', this.decodedToken.SUB)
          this.$router.push('/home')
        })
        .catch(err => {
          console.log(err)
        })
    }
  }
}
</script>
