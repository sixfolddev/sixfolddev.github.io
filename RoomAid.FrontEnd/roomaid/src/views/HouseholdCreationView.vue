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
        :rules="ZipRules"
        required
      ></v-text-field>
       <v-text-field
        id="rent"
        v-model="Rent"
        label="Rent"
        type="double"
        required
      ></v-text-field>
    </v-form>
    </v-col>
    </v-row>
    <v-btn id="create" class="button" @click="Create">Create</v-btn>
    <div v-if="Loading" class="text-center">
      <v-progress-circular
        :size="100"
        color="primary"
        indeterminate
      ></v-progress-circular>
    </div>

    <v-dialog v-model="dialog" max-width="800" :persistent="true">
      <v-card>
        <v-card-title class="justify-center">Creation Status</v-card-title>

        <v-card-text>
          {{ DialogMessage }}
        </v-card-text>

        <v-card-actions>
          <v-spacer></v-spacer>

          <v-btn color="green" text @click.stop="Finish()">
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
  data () {
    return {
      Loading: false,
      Requester: 'albertdu233@gmail.com',
      StreetAddress: '',
      AptNumber: '0',
      City: '',
      Zip: '',
      Rent: 0.00,
      FormStatus: false,
      DialogMessage: '',
      dialog: false,
      DialogHeadline: '',
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
        (v) => v.length === 5 || 'zip code must be 5 digits'
      ]
    }
  },
  methods: {
    Create () {
      const formValid = this.$refs.form.validate()
      if (formValid) {
        this.Loading = true
        fetch(`${this.$hostname}/api/householdmanagement/createhousehold`, {
          method: 'POST',
          mode: 'cors',
          headers: {
            Accept: 'application/json',
            'Content-Type': 'application/json'
          },

          body: JSON.stringify({
            RequesterEmail: this.$data.Requester,
            Rent: this.$data.Rent,
            StreetAddress: this.$data.StreetAddress,
            City: this.$data.City,
            ZipCode: this.$data.Zip,
            Suitnumber: this.$data.Aptnumber
          })
        }).then((response) => {
          this.Loading = false
          if (response > 401) {
            throw Error('response error')
          }
          return response.json()
        })
          .then((data) => {
            if (data === true) {
              this.Loading = false
              this.dialog = true
              this.DialogMessage =
                'Household creation finished!'
              this.FormStatus = true
            } else {
              var errorMesssage = 'Internal error!'
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
    Finish () {
      this.dialog = false
    }
  }
}
</script>

<style scoped>
</style>
