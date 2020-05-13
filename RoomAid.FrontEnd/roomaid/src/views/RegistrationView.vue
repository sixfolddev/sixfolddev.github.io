<template>
  <div>
    <h1>User Registration</h1>
    <v-row justify="center">
        <v-col cols="12" sm="10" md="8" lg="6">
    <v-form ref="form" :lazy-validation="false" justify="center">
        <v-text-field
        id="email"
        v-model="Email"
        label="Email"
        :rules="EmailRules"
        required
      ></v-text-field>
      <v-text-field
        id="firstname"
        v-model="Firstname"
        label="First Name"
        :rules="FirstnameRules"
        required
      ></v-text-field>
      <v-text-field
        id="lastname"
        v-model="Lastname"
        label="Last name"
        :rules="LastnameRules"
        required
      ></v-text-field>
      <v-text-field
        id="password"
        @keyup="Validate"
        type="password"
        v-model="Password"
        label="Password"
        :rules="PasswordRules"
      ></v-text-field>
      <v-text-field
        id="repeatPassword"
        @keyup="Validate"
        type="password"
        v-model="RepeatPassword"
        label="Repeat Password"
        :rules="RepeatPasswordRules"
      ></v-text-field>
    </v-form>
     <v-menu
        ref="menu"
        v-model="menu"
        :close-on-content-click="false"
        :return-value.sync="DOB"
        transition="scale-transition"
        offset-y
        min-width="290px"
      >
        <template v-slot:activator="{ on }">
          <v-text-field
            v-model="DOB"
            label="Pick a date of birth"
            readonly
            v-on="on"
          ></v-text-field>
        </template>
        <v-date-picker v-model="DOB" :format= dateFormat no-title scrollable :selectableYearRange="{from: 1985, to: 2020} ">
          <v-spacer></v-spacer>
          <v-btn text color="primary" @click="menu = false">Cancel</v-btn>
          <v-btn text color="primary" @click="$refs.menu.save(DOB)">OK</v-btn>
        </v-date-picker>
      </v-menu>
    </v-col>
    </v-row>
    <v-btn id="submit" class="button" @click="Register">Submit</v-btn>
    <div v-if="Loading" class="text-center">
      <v-progress-circular
        :size="100"
        color="primary"
        indeterminate
      ></v-progress-circular>
    </div>
    <v-dialog v-model="dialog" max-width="800" :persistent="true">
      <v-card>
        <v-card-title class="justify-center">Registration Status</v-card-title>

        <v-card-text>
          {{ DialogMessage }}
        </v-card-text>

        <v-card-actions>
          <v-spacer></v-spacer>

          <v-btn color="green" text @click.stop="GoToLogin()">
            Accept
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </div>
</template>

<script>
import 'vue-date-pick/dist/vueDatePick.css'
export default {
  name: 'RegistrationView',

  computed: {
    GetPassword () {
      return this.Password
    },
    GetRepeatedPassword () {
      return this.RepeatPassword
    }
  },
  data () {
    return {
      Loading: false,
      dateFormat: 'yyyy.MM.dd',
      Email: '',
      Firstname: '',
      Lastname: '',
      DOB: new Date().toISOString().substr(0, 10),
      menu: false,
      modal: false,
      menu2: false,
      Password: '',
      RepeatPassword: '',
      FormStatus: false,
      dialog: false,
      DialogMessage: '',
      formValid: '',
      // Rules callback
      FirstnameRules: [
        (v) => !!v || 'First name is required',
        (v) => v.length < 200 || 'First name must be less than 200 characters'
      ],
      LastnameRules: [
        (v) => !!v || 'Last name is required',
        (v) => v.length < 200 || 'Last name must be less than 200 characters'
      ],
      EmailRules: [
        (v) => !!v || 'Email is required',
        (v) => !v || /^\w+([.-]?\w+)*@\w+([.-]?\w+)*(\.\w{2,3})+$/.test(v) || 'E-mail must be valid'
      ],
      PasswordRules: [
        () =>
          this.GetPassword === this.RepeatPassword ||
          'Passwords are not equal',
        (v) => !!v || 'Password is required',
        (v) => v.length >= 12 || 'Password must be no less than 12',
        (v) => v.length < 2000 || 'Password  must be less than 2000'
      ],
      RepeatPasswordRules: [
        () =>
          this.GetPassword === this.RepeatPassword || 'Passwords are not equal',
        (v) => !!v || 'Password is required',
        (v) => v.length >= 12 || 'Password must be no less than 12',
        (v) => v.length < 2000 || 'Password  must be less than 2000'
      ]
    }
  },
  methods: {
    Validate () {
      this.$refs.form.validate()
    },
    Register () {
      const formValid = this.$refs.form.validate()
      if (formValid) {
        this.Loading = true
        fetch(`${this.$hostname}/api/registration/registerUser`, {
          method: 'POST',
          mode: 'cors',
          headers: {
            Accept: 'application/json',
            'Content-Type': 'application/json'
          },
          body: JSON.stringify({
            Email: this.$data.Email,
            Firstname: this.$data.Firstname,
            Lastname: this.$data.Lastname,
            Password: this.$data.Password,
            RepeatPassword: this.$data.RepeatPassword,
            DateofBirth: this.$data.DOB
          })
        }).then((response) => {
          // Remove loading on response.
          this.Loading = false
          if (response > 401) {
            throw Error('response error')
          }
          // Process response as json
          return response.json()
        })
          .then((data) => {
            if (data === true) {
              this.dialog = true
              this.DialogMessage =
                'Registration finished!'
              this.FormStatus = true
            } else {
              this.Loading = false
              var errorMesssage = data.Message
              this.dialog = true
              this.DialogMessage = errorMesssage
            }
          })
          .catch((error) => {
            this.Loading = false
            console.log(error)
            this.dialog = true
            this.DialogMessage = 'Fetch data failed!: contact system admin!'
          })
      } else {
        this.Loading = false
        this.DialogMessage = 'Please make sure all fields are filled and valid!'
        this.dialog = true
      }
    },
    GoToLogin () {
      this.dialog = false
      if (this.FormStatus === true) {
        this.$router.push('Login').catch((err) => err)
      }
    }
  }
}
</script>
<style scoped>
</style>
