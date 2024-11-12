import "./App.css"
import Header from "./components/Header"
import Footer from "./components/Footer"
import Home from "./pages/Home"
const App = () => {

  return (
    <div className="font-[inter] h-screen max-w-screen-xl mx-auto px-40 pt-6 pb-12 flex flex-col justify-between">
      <Header/>
      <Home/>
      <Footer/>
    </div>
  )
}

export default App
