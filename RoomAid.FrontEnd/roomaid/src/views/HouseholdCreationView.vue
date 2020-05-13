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
        :rules="ZipRules"
        required
      ></v-text-field>
       <v-text-field
        id="rent"
        v-model="Rent"
        label="Rent"
        :rules="RentRules"
        type="double"
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
<v-btn id="Create" @click="CreateService">Create</v-btn>
  </div>
</template>

<script>
export default {
  name: 'HouseholdCreationView',
  components: { },
  data () {
    return {
      Loading: false,
      Requester: '',
      StreetAddress: '',
      AptNumber: '',
      City: '',
      Zip: '',
      Rent: 0.00,
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
      ],
      RentRules: [
        (v) => !0.00 || 'Rent is required'
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
            StreetAddress: this.$data.Email,
            City: this.$data.City,
            ZipCode: this.$zip,
            Suitnumber: this.$data.Aptnumber
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
                'Household creation finished!'
              this.FormStatus = true
            } else {
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
    }
  }
}
</script>

<style scoped>
</style>
