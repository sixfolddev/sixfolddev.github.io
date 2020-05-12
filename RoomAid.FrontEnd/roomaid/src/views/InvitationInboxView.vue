<template>
<div id = "InvitationInboxView"> <!-- Start inbox container -->
  <v-container fluid>
    <InboxNavPane/>
    <InboxContainer/>
  <div class="messages"> <!-- Start inbox container -->
    <!-- Start message items -->
  </div>
  </v-container>
</div>
</template>

<script>
// @ is an alias to /src
import InboxNavPane from '@/components/InboxNavPane.vue'
import InboxContainer from '@/components/InboxContainer.vue'

export default {
  name: 'InvitationInboxView',
  components: {
    InboxNavPane,
    InboxContainer
  },
  data: () => ({
    type: 'invitation'
  }),
  created () {
    this.type = this.updateMessageType()
  },
  methods: {
    updateMessageType () {
      this.$store.dispatch('updateMessageType', this.type)
    }
  }
}
</script>

<style scoped>

</style>
