import React, { FC } from "react"
import Header from "../components/Header"
import { Outlet } from "react-router-dom"
import Footer from "../components/Footer"

const Layout: FC = () => {
  return (
    <div
      className="font-[inter] h-screen max-w-screen-xl mx-auto px-40 pt-6 pb-12 flex flex-col justify-between">
      <Header />
      <Outlet />
      <Footer />
    </div>
  )
}

export default Layout