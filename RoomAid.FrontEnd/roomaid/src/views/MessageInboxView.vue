<template>
<div id = "MessageInboxView"> <!-- Start inbox container -->
  <v-container fluid>
    <InboxNavBar/>
    <InboxContainer/>
  </v-container>
</div>
</template>

<script>
// @ is an alias to /src
import InboxNavBar from '@/components/InboxNavBar.vue'
import InboxContainer from '@/components/InboxContainer.vue'

export default {
  name: 'MessageInboxView',
  components: {
    InboxNavBar,
    InboxContainer
  },
  data: () => ({
    type: 'general'
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
