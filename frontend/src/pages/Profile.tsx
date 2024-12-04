import type { FC } from "react"
import Input from "../components/input/Input"
import logout from "../assets/logout.svg"
import FormCard from "../components/FormCard"
import addEventCard from "../assets/addEventCard.svg"
import { Link } from "react-router-dom"

const Profile: FC = () => {
  const mock: Array<FormCardProps> = [{
      id: 1,
      title: "Цифровая сила предприятия с SILA Union 2024",
      date: "12.11.2024",
      time: "10:00",
      members: 72,
      description: "Конференция «Цифровая сила предприятия с SILA Union» – крупнейшее отраслевое мероприятие в области бизнес-моделирования и цифровой трансформации, пройдет 12 ноября 2024 г. на самой инновационной площадке г. Москва. "
    },
    {
      id: 2,
      title: "Цифровая сила предприятия с SILA Union 2024",
      date: "12.11.2024",
      time: "10:00",
      members: 72,
      description: "Конференция «Цифровая сила предприятия с SILA Union» – крупнейшее отраслевое мероприятие в области бизнес-моделирования и цифровой трансформации, пройдет 12 ноября 2024 г. на самой инновационной площадке г. Москва. "
    },
    {
      id: 3,
      title: "Цифровая сила предприятия с SILA Union 2024",
      date: "12.11.2024",
      time: "10:00",
      members: 72,
      description: "Конференция «Цифровая сила предприятия с SILA Union» – крупнейшее отраслевое мероприятие в области бизнес-моделирования и цифровой трансформации, пройдет 12 ноября 2024 г. на самой инновационной площадке г. Москва. "
    }
    ]
  return (
    <>
      <div className={"main-container flex flex-col gap-8"}>
        <div className={""}>
          <h1 className={"font-semibold text-[32px]"}>Личный кабинет</h1>
          <p className={"mt-3 text-secondary-text"}>Здесь вы можете изменить свою электронную почту,
            добавить имя и фамилию или управлять настройками сервиса.</p>
        </div>
        <div className={"flex flex-col gap-4"}>
          <Input type={"text"} label={"d.ivanov@rostatom.ru"} />
          <Input type={"text"} label={"Введите имя"} />
          <Input type={"text"} label={"Введите фамилию"} />
        </div>
        <button className={"base-btn w-[248px] bg-danger"}>
          <img src={logout} alt="" className={"inline-block mr-2.5 pb-1"} />
          Выйти из аккаунта
        </button>
      </div>
      <div className={"main-container flex flex-col gap-8"}>
        <h1 className={"font-semibold text-[32px]"}>Формы бронирования</h1>
        <p className={"mt-3 text-secondary-text"}>
          Здесь вы можете просматривать, редактировать
          и удалять созданные вами мероприятия, а также собирать данные о посетителях..
        </p>
        <div className={"flex gap-8 flex-wrap"}>
          {
            mock.map((card) =>
              (
                <div key={card.id}>
                  <FormCard
                    id={card.id}
                    title={card.title}
                    date={card.date}
                    time={card.time}
                    members={card.members}
                    description={card.description}
                  />
                </div>
              )
            )
          }
          <Link to={"/createform"}>
            <button className={"bg-secondary-bg size-[266px] rounded-2xl"}>
              <img src={addEventCard} alt="" className={"m-auto"} />
            </button>
          </Link>
        </div>
      </div>
    </>
  )
}

export default Profile