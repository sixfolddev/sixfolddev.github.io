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
        v-model="Lastrname"
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
        id="pepeatPassword"
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
        :return-value.sync="date"
        transition="scale-transition"
        offset-y
        min-width="290px"
      >
        <template v-slot:activator="{ on }">
          <v-text-field
            v-model="date"
            label="Pick a date of birth"
            readonly
            v-on="on"
          ></v-text-field>
        </template>
        <v-date-picker v-model="date" no-title scrollable>
          <v-spacer></v-spacer>
          <v-btn text color="primary" @click="menu = false">Cancel</v-btn>
          <v-btn text color="primary" @click="$refs.menu.save(date)">OK</v-btn>
        </v-date-picker>
      </v-menu>
    </v-col>
    </v-row>
    <v-btn id="submit" class="button" @click="Submit">Submit</v-btn>
    <div v-if="Loading" class="text-center">
      <v-progress-circular
        :size="100"
        color="primary"
        indeterminate
      ></v-progress-circular>
    </div>
    <!-- Dialog to display the status of the form submission -->
    <v-dialog v-model="dialog" max-width="800" :persistent="true">
      <v-card>
        <v-card-title class="headline">Registration Status</v-card-title>

        <v-card-text>
          {{ DialogMessage }}
        </v-card-text>

        <v-card-actions>
          <v-spacer></v-spacer>

          <v-btn color="green darken-1" text @click.stop="GoToLogin()">
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
  components: { },
  data () {
    return {
      Loading: false,
      Username: '',
      Firstname: '',
      LastName: '',
      date: new Date().toISOString().substr(0, 10),
      menu: false,
      modal: false,
      menu2: false,
      email: '',
      DOB: '2019-01-01',
      Password: '',
      RepeatPassword: '',
      FormStatus: false,
      dialog: false,
      DialogMessage: '',
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
    }
  }
}
</script>

<style scoped>
</style>
