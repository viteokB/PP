import type IInput from "../../model/types"
import BaseInput from "./BaseInput/BaseInput"

const Input = ({type, label, onClick}: IInput)=> {
  switch (type) {
    case 'text':
      return <BaseInput type={type} label={label} onClick={onClick} />;
    case 'textarea':
      return <></>;
    case 'checkbox':
      return <></>
    default:
      return null;
  }
}

export default Input