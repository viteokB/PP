import type { FC } from "react"
import Input from "../components/input/Input"

const CreateForm: FC = () => {
  return (
    <div className={"main-container flex flex-col gap-8"}>
      <h2 className={"text-secondary-text"}>Шаг 1 из 2</h2>
      <div className={""}>
        <h1 className={"font-semibold text-[32px]"}>Создание формы бронирования</h1>
        <p className={"mt-3"}>Введите основную информацию о мероприятии, а затем укажите его временные
          интервалы и максимальное количество посетителей.</p>
      </div>
      <div className={"flex flex-col gap-4"}>
        <Input type={"text"} label={"Ваша почта"} />
        <Input type={"text"} label={"Название мероприятия"} />
      </div>
      <button className={"base-btn"}>Далее</button>
    </div>

  )
}

export default CreateForm