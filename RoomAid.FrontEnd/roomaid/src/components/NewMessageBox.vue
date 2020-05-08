<template>
  <div id="newmessagebox">
      <v-layout>
        <v-flex>
          <v-app style="background-color: white;" class="ma-n4">
            <v-container>
              <template>
                <v-row>To: {{sendto}}</v-row>
                <v-divider></v-divider>
                <v-textarea v-model="content"
                counter
                :rules="rules"
                :auto-grow="true">
                </v-textarea>
                <v-row></v-row> <!-- Spacer between message and button-->
                <v-row>
                  <v-layout justify-end>
                    <v-btn id="reply" text class="success mx-0 mt-3" @click="sendMessage">SEND</v-btn>
                  </v-layout>
                </v-row>
              </template>
            </v-container>
          </v-app>
        </v-flex>
      </v-layout>
    </div>
</template>

<script>
export default {
  data: () => ({
    userid: 0,
    sendtoid: 0,
    sendto: '',
    content: '',
    messagetype: '',
    prevmessageid: 0,
    rules: [v => v.length <= 2000 || 'Max 2000 charcters'],
    isreply: false
  }),
  created () {
    // Get user's id from persisted store
    this.userid = this.$store.getters.userid
    // Get other user full name from persisted store
    this.sendtoid = this.$store.getters.otheruserid
    // Get message receiver full name from persisted store
    this.sendto = this.$store.getters.otherusername
    // Get type of message (general or invitation) from persisted store
    this.messagetype = this.$store.getters.messagetype
    // Get id of previous message
    this.prevmessageid = this.$store.getters.prevmessageid
  },
  methods: {
    sendMessage () {
      const uri = `${this.$hostname}/api/inbox/${this.userid}/${this.prevmessageid}/reply/${this.messagetype}/${this.sendtoid}` // UPDATE LATER; THIS IS ONLY FOR REPLIES RIGHT NOW
      const h = new Headers()
      h.append('Access-Control-Allow-Origin', '*')
      h.append('Access-Control-Allow-Methods', 'POST')
      h.append('Access-Control-Allow-Headers', 'Content-Type, Access-Control-Allow-Headers, Authorization, X-Requested-With')
      h.append('Content-Type', 'application/json')
      const req = new Request(uri, {
        method: 'POST',
        headers: h,
        mode: 'cors',
        body: JSON.stringify(this.content)
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
          console.log(data) // Boolean return value
        })
        .catch(err => {
          console.log(err)
        })
      if (this.messagetype === 'general') {
        this.$router.push('/inbox/messages')
      } else {
        this.$router.push('/inbox/invitations')
      }
    }
  }
}
</script>
