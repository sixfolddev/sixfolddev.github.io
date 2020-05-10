<template>
  <div class="home">
    <div class="logo">
      <img alt="RoomAid" src="../assets/Room-Aid-Logo_Final.png">
    </div>
    <HelloWorld msg=""/>
  </div>
</template>

<script>
// @ is an alias to /src
import HelloWorld from '@/components/HelloWorld.vue'

export default {
  name: 'Home',
  components: {
    HelloWorld
  }
}
</script>
