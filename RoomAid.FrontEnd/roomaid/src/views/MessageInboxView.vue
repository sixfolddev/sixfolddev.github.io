<template>
<div id = "MessageInboxView"> <!-- Start inbox container -->
  <v-container fluid>
    <InboxNavPane/>
    <InboxContainer/> <!-- messages='messages' -->
  </v-container>
</div>
</template>

<script>
// @ is an alias to /src
import InboxNavPane from '@/components/InboxNavPane.vue'
import InboxContainer from '@/components/InboxContainer.vue'

export default {
  name: 'MessageInboxView',
  components: {
    InboxNavPane,
    InboxContainer
  },
  data: () => ({
    type: 'general'
    // messages: []
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
