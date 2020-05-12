<template>
  <div>
    <h1>Create a New HouseHold</h1>
    <v-row justify="center">
        <v-col cols="12" sm="10" md="8" lg="6">
    <v-form ref="form" :lazy-validation="false" justify="center">
        <v-text-field
        id="streetAddress"
        v-model="StreetAddress"
        label="Street Address"
        :rules="StreetAddressRules"
        required
      ></v-text-field>
          <v-text-field
        id="aptNumber"
        v-model="AptNumber"
        label="Apt Number"
        :rules="AptNumberRules"
        optional
      ></v-text-field>
      <v-text-field
        id="city"
        v-model="City"
        label="City"
        :rules="CityRules"
        required
      ></v-text-field>
      <v-text-field
        id="zip"
        v-model="Zip"
        label="Zipcode"
        :rules="zipRules"
        required
      ></v-text-field>
    </v-form>
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
export default {
  name: 'HouseholdCreationView',
  components: { },
  data () {
    return {
      Loading: false,
      StreetAddress: '',
      AptNumber: '',
      City: '',
      Zip: '',
      FormStatus: false,
      dialog: false,
      DialogMessage: '',
      // Rules callback
      StreetAddressRules: [
        (v) => !!v || 'Street Address is required',
        (v) => v.length < 200 || 'Street Address must be less than 200 characters'
      ],
      CityRules: [
        (v) => !!v || 'City is required',
        (v) => v.length < 200 || 'City must be less than 200 characters'
      ],
      ZipRules: [
        (v) => !!v || 'Zipcode is required',
        (v) => v.length !== 5 || 'zip code must be 5 digits'
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
